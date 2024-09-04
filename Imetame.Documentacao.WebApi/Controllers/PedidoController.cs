using DocumentFormat.OpenXml.Office.Word;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;

//using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OpenIddict.Validation.AspNetCore;
using System.Linq;
using System.Text;
using static Imetame.Documentacao.Domain.Models.HistoricoDocumento;
using static Imetame.Documentacao.Domain.Models.RelatorioModel;
using Pedido = Imetame.Documentacao.Domain.Entities.Pedido;

namespace Imetame.Documentacao.WebApi.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
	public class PedidoController : Controller
	{
		private readonly IBaseRepository<Pedido> _repPedido;
		private readonly IBaseRepository<ColaboradorxPedido> _repColaboradorxPedido;
		private readonly IBaseRepository<Colaborador> _repColaborador;
		private readonly IBaseRepository<Documento> _repDocumento;
		private readonly IConfiguration _configuration;
		private readonly DestraController _destraController;
		public PedidoController(IBaseRepository<Pedido> repPedido, IConfiguration configuration, IBaseRepository<ColaboradorxPedido> repColaboradorxPedido, DestraController destraController, IBaseRepository<Colaborador> repColaborador, IBaseRepository<Documento> repDocumento)
		{
			_repPedido = repPedido;
			_configuration = configuration;
			_repColaboradorxPedido = repColaboradorxPedido;
			_destraController = destraController;
			_repColaborador = repColaborador;
			_repDocumento = repDocumento;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllPaginated(int page = 1, int pageSize = 10, string texto = "")
		{
			try
			{
				if (!ModelState.IsValid)
					throw new Exception("Parametros necessarios nao informados");

				int offset = (page - 1) * pageSize;

				IQueryable<Pedido> query = _repPedido.SelectContext().AsNoTracking().Include(m => m.Credenciadora)
															 .OrderBy(x => x.NumPedido);
				if (!string.IsNullOrEmpty(texto))
					query = query.Where(q => q.Unidade.Contains(texto) || q.NumPedido.Contains(texto));

				int totalCount = await query.CountAsync();

				IList<Pedido> listPedidos = query.Skip(offset)
															 .Take(pageSize).ToList();

				PaginatedResponse<Pedido> paginatedResponse = new PaginatedResponse<Pedido>
				{
					TotalCount = totalCount,
					Page = page,
					PageSize = pageSize,
					Data = listPedidos
				};
				return Ok(paginatedResponse);
			}
			catch (Exception ex)
			{
				return BadRequest(ErrorHelper.GetException(ex));
			}
		}


		#region FUNÇÕES DE APOIO - MATHEUS MONFREIDES FARTEC SISTEMAS

		#region GET ALL
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			if (!ModelState.IsValid)
				throw new Exception("Parametros necessarios nao informados");

			List<Pedido> listaPedido = await _repPedido.SelectContext()
															.ToListAsync();
			return Ok(listaPedido);
		}
		#endregion GET ALL

		#region GET
		[HttpGet("{id}")]
		public async Task<IActionResult> GetItem(Guid id)
		{
			if (!ModelState.IsValid)
				throw new Exception("Parametros necessarios nao informados");

			Pedido Pedido = await _repPedido.SelectContext().AsNoTracking().Include(m => m.Credenciadora)
															.Where(e => e.Id.Equals(id))
															.FirstAsync();
			return Ok(Pedido);
		}
		#endregion GET ALL        


		#endregion

		#region FUNÇÕES CRUD - MATHEUS MONFREIDES FARTEC SISTEMAS
		[HttpPost]
		public async Task<IActionResult> Add([FromBody] Pedido model, CancellationToken cancellationToken)
		{
			try
			{
				StringBuilder erro = new StringBuilder();
				if (!ModelState.IsValid)
				{
					erro = ErrorHelper.GetErroModelState(ModelState.Values);
					throw new Exception("Falha ao Salvar Dados.\n" + erro);
				}
				model.Id = new Guid();
				await _repPedido.SaveAsync(model);

				return Ok(model);
			}
			catch (Exception ex)
			{
				return BadRequest(ErrorHelper.GetException(ex));
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] Pedido model, CancellationToken cancellationToken)
		{
			try
			{
				StringBuilder erro = new StringBuilder();
				if (!ModelState.IsValid)
				{
					erro = ErrorHelper.GetErroModelState(ModelState.Values);
					throw new Exception("Falha ao Salvar Dados.\n" + erro);
				}

				await _repPedido.UpdateAsync(model);


				return Ok(model);
			}
			catch (Exception ex)
			{
				return BadRequest(ErrorHelper.GetException(ex));
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				Pedido Pedido = await _repPedido.SelectAsync(id) ?? throw new Exception("Erro ao apagar o Pedido com o id fornecido.");

				await _repPedido.DeleteAsync(Pedido);

				return Ok(new ApiResponse
				{
					Success = true,
					Message = "Pedido excluido com sucesso"
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		#endregion

		#region DADOS RELATÓRIO
		[HttpGet]
		public async Task<IActionResult> GetDadosCheckList(Guid idPedido, string codOs)
		{
			try
			{
				// TODO - DEVIDO AS DIVERSAS ALTERAÇÕES NO FORMATO DO RELATORIO NECESSARIO REFATORAR O CÓDIGO
				if (!ModelState.IsValid)
					throw new Exception("Parametros necessarios nao informados");

				// RECEBE OS COLABORADORES RELACIONADOS AO PEDIDO E A OS INFORMADA
				IList<Colaborador> colaboradores = await _repColaborador.SelectContext()
																		.AsNoTracking()
																		.Where(x => x.ColaboradorxPedido.Select(y => y.CXP_IDPEDIDO).Contains(idPedido) && x.ColaboradorxPedido.Select(y => y.CXP_NUMEROOS).Contains(codOs))
																		.Include(x => x.ColaboradorxPedido)
																			.ThenInclude(x => x.Pedido)
																		.Include(y => y.ColaboradorxAtividade)
																			.ThenInclude(x => x.AtividadeEspecifica)
																		.ToListAsync();

				// RECEBE OS DOCUMENTOS CADASTRADOS COMO OBRIGATORIOS
				IList<Documento> documentosBasicos = await _repDocumento.SelectByCondition(x => x.Obrigatorio == true).ToListAsync();

				IList<ChecklistModel> colaboradoresModel = new List<ChecklistModel>();

				foreach (Colaborador colaborador in colaboradores)
				{
					ChecklistModel checklistModel = new ChecklistModel()
					{
						Nome = colaborador!.Nome,
						//Matricula = colaborador!.Matricula,
						//Equipe = colaborador!.Nome_Equipe,
						//DataAdmissao = colaborador!.DataAdmissao,
						//Rg = colaborador!.Rg,
						//OrdemServico = $"{colaborador!.Codigo_OS.Trim()} - {colaborador!.Nome_OS}",
						//Cpf = colaborador!.Cpf,
						//NumPedido = colaborador.ColaboradorxPedido.Select(x => $"{x.Pedido!.NumPedido} - {x.Pedido.Unidade}").FirstOrDefault(),
						Documentos = new List<CheckDocumento>(),
					};

					HistoricoDestraApiModel historicoDocumentoDestras = new HistoricoDestraApiModel() { LISTA = new List<HistoricoDocumentoDestra>() };
					var jsonResponseHistorico = await _destraController.GetHistoricoDocumentosByColaborador(colaborador.Cpf);
					historicoDocumentoDestras = JsonConvert.DeserializeObject<HistoricoDestraApiModel>(jsonResponseHistorico);
					// AGRUPA POR DOCUMENTO E PEGA APENAS O MAIS RECENTE
					historicoDocumentoDestras.LISTA = historicoDocumentoDestras.LISTA!
																		.GroupBy(d => d.idDocto)
																		.Select(g => g.OrderByDescending(d => d.id).First())
																		.ToList();

					IList<AtividadeEspecifica> atividades = colaborador.ColaboradorxAtividade.Select(y => y.AtividadeEspecifica!).ToList();

					foreach (Documento docBasico in documentosBasicos)
					{
						HistoricoDocumentoDestra historicoDestraApiModel = historicoDocumentoDestras.LISTA.Where(x => x.idDocto.ToString() == docBasico.IdDestra).FirstOrDefault();

						// TRATAR PARA QUE UM DOCUMENTO MARCADADO COMO OBRIGATÓRIO NÃO SEJA SEJA REPETIDO NO RELATÓRIO
						if (!checklistModel.Documentos.Any(x => x.IdDestra.ToString() == docBasico.IdDestra))
						{
							CheckDocumento checkDocumento;
							if (historicoDestraApiModel != null)
							{
								checkDocumento = new CheckDocumento()
								{
									IdDestra = historicoDestraApiModel.idDocto,
									//Atividade = "DOCUMENTO BÁSICO",
									impeditivo = historicoDestraApiModel!.impeditivo!,
									nome = historicoDestraApiModel!.nomeDocto!,
									Status = historicoDestraApiModel.status,
									validade = historicoDestraApiModel.validade!,
								};
							}
							else
							{
								checkDocumento = new CheckDocumento()
								{
									IdDestra = Convert.ToInt32(docBasico.IdDestra),
									//Atividade = "DOCUMENTO BÁSICO",
									impeditivo = "N",
									nome = docBasico!.DescricaoDestra!,
									Status = -2,
									validade = "Não Informado",
								};
							}
							checklistModel.Documentos.Add(checkDocumento);
						}
					}
					// GUARDA ATIVIDADE COM A FORMATAÇÃO SOLICITADA PARA A EXIBIÇÃO NO RELATÓRIO
					string atividadeFormatada = "";
					int numAtividade = 1;
					foreach (AtividadeEspecifica atividade in atividades)
					{
						// VERIFICAR SE É O ULTIMO ITEM DE ATIVIDADES PARA QUE NÃO PULE A LINHA
						if (numAtividade == atividades.Count)
							atividadeFormatada = atividadeFormatada + $"{numAtividade}) {atividade.Descricao}";
						else
							atividadeFormatada = atividadeFormatada + $"{numAtividade}) {atividade.Descricao}\n";

						numAtividade++;

						ListaDocumentosDestraModel DocsPorAtividade = new ListaDocumentosDestraModel() { LISTA = new List<DocumentosDestra>() };
						var jsonResponse = await _destraController.GetDocumentosRequeridos(atividade!.IdDestra.ToString());
						DocsPorAtividade = JsonConvert.DeserializeObject<ListaDocumentosDestraModel>(jsonResponse);

						foreach (var docDestra in DocsPorAtividade!.LISTA)
						{
							// RECEBE O HISTORICO DESTRA REFERENTE AO DOCUMENTO EN QUESTÃO
							HistoricoDocumentoDestra historicoDestraApiModel = historicoDocumentoDestras.LISTA!.Where(x => x.idDocto == docDestra.codigo).FirstOrDefault();
							CheckDocumento checkDocumento;
							// TRATAR PARA QUE UM DOCUMENTO MARCADADO COMO OBRIGATÓRIO NÃO SEJA SEJA REPETIDO NO RELATÓRIO
							if (!checklistModel.Documentos.Any(x => x.IdDestra == docDestra.codigo))
							{
								if (historicoDestraApiModel != null)
								{
									checkDocumento = new CheckDocumento()
									{
										IdDestra = historicoDestraApiModel.idDocto,
										impeditivo = docDestra.impeditivo,
										nome = docDestra.nome,
										Status = historicoDestraApiModel.status,
										validade = historicoDestraApiModel.validade!,
									};
								}
								else
								{
									checkDocumento = new CheckDocumento()
									{
										IdDestra = docDestra.codigo,
										impeditivo = docDestra.impeditivo,
										nome = docDestra.nome,
										Status = -2,
										validade = "Não Informado",
									};
								}
								checklistModel.Documentos.Add(checkDocumento);
							}
						}
					}
					checklistModel.Atividade = atividadeFormatada;
					//checklistModel.ItensDestra = DocsPorAtividade!.LISTA.Distinct().Select(x => x.nome).ToList();
					colaboradoresModel.Add(checklistModel);
				}
				return Ok(colaboradoresModel);
			}
			catch (Exception ex)
			{
				return BadRequest(ErrorHelper.GetException(ex));
			}
		}
		#endregion
	}
}
