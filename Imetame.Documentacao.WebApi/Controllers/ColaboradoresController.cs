using Dapper;
using DocumentFormat.OpenXml.Drawing;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Imetame.Documentacao.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly IColaboradorRepository _repository;
        private readonly IBaseRepository<Domain.Entities.Processamento> _repProcessamento;
        private readonly IBaseRepository<Domain.Entities.Colaborador> _repColaborador;
        protected readonly SqlConnection conn;
        private readonly IConfiguration _configuration;


        public ColaboradoresController(IColaboradorRepository repository, IBaseRepository<Domain.Entities.Processamento> repProcessamento, IConfiguration configuration, IBaseRepository<Domain.Entities.Colaborador> repColaborador)
        {
            _repository = repository;
            _configuration = configuration;
            _repProcessamento = repProcessamento;
            conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            _repColaborador = repColaborador;
        }

        [HttpGet("{idProcessamento}")]
        public async Task<IActionResult> GetColaboradoresPorOs(Guid idProcessamento, CancellationToken cancellationToken)
        {
            try
            {
                var resp = await _repository.ListaAsync(idProcessamento, cancellationToken);

                Domain.Entities.Processamento? processamento = await _repProcessamento.SelectContext().Where(m => m.Id == idProcessamento).FirstOrDefaultAsync();

                #region CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS
                conn.Open();
                var sql = @"SELECT distinct [empresa] as Empresa
                      ,[numcad] as NumCad
                      ,[numcracha] as NumCracha
                      ,[status] as Status
                      ,[nomefuncionario] as Nome
                      ,[cpf] as Cpf
                      ,[funcaoatual] as FuncaoAtual
                      ,[funcaoinicial] as FuncaoInicial
                      ,[dataafastamento] as DataAfastamento
                      ,[dataadmissao] as DataAdmissao
                      ,[datanascimento] as DataNascimento
                      ,[equipe] as Equipe
                      ,[perfil] as Perfil
                      ,[endereco] as Endereco
                      ,[numero] as Numero
                      ,[bairro] as Bairro
                      ,[cidade] as Cidade
                      ,[cep] as Cep
                      ,[ddd] as Ddd
                      ,[numtel] as NumTel
                      ,[ddd2] as Ddd2
                      ,[numtel2] as NumTel2
                      ,[estado] as Estado
                      ,[tempoempresaanos] as TempoEmpresaAnos
                      ,[tempoempresaanosint] as TempoEmpresaAnosInt
                      ,[tempoempresamesesint] as TempoEmpresaMesesInt
                      ,[tempoempresatexto] as TempoEmpresaTeexto
	 

                  FROM [DW_IMETAME_NOVA_OS].[dbo].VW_FUSION_GP_COLABORADOR (nolock)  COLAB

                  join DADOSADV_LUC..ZNB010 (nolock) ZNB ON ZNB.ZNB_MATRIC = COLAB.[numcad] AND ZNB.D_E_L_E_T_='' AND ZNB.ZNB_DTFIM>GETDATE()-30

                  WHERE ZNB_OS in @Oss
                order by Nome";
                #endregion CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS

                var lista = (await this.conn.QueryAsync<ColaboradorModel>(sql, new { Oss = processamento.Oss }));
                return Ok(lista);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated(int page = 1, int pageSize = 10, string texto = "")
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Parametros necessarios nao informados");

                int offset = (page - 1) * pageSize;

                IQueryable<Colaborador> query = _repColaborador.SelectContext()
                                                             .OrderBy(x => x.Nome);
                if (!string.IsNullOrEmpty(texto))
                    query = query.Where(q => q.Nome.Contains(texto) || q.Matricula.Contains(texto));

                int totalCount = await query.CountAsync();

                IList<Colaborador> listColaboradors = query.Skip(offset)
                                                             .Take(pageSize).ToList();

                PaginatedResponse<Colaborador> paginatedResponse = new PaginatedResponse<Colaborador>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = listColaboradors
                };
                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpGet()]
        public async Task<IActionResult> GetColaboradores(int page = 1, int pageSize = 10, string texto = "")
        {
            try
            {
                int offset = (page - 1) * pageSize;
                #region CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS
                conn.Open();
                var sql = @"SELECT distinct [empresa] as Empresa
                      ,[numcad] as NumCad
                      ,[numcracha] as NumCracha
                      ,[status] as Status
                      ,[nomefuncionario] as Nome
                      ,[cpf] as Cpf
                      ,[funcaoatual] as FuncaoAtual
                      ,[funcaoinicial] as FuncaoInicial
                      ,[dataafastamento] as DataAfastamento
                      ,[dataadmissao] as DataAdmissao
                      ,[datanascimento] as DataNascimento
                      ,[equipe] as Equipe
                      ,[perfil] as Perfil
                      ,[endereco] as Endereco
                      ,[numero] as Numero
                      ,[bairro] as Bairro
                      ,[cidade] as Cidade
                      ,[cep] as Cep
                      ,[ddd] as Ddd
                      ,[numtel] as NumTel
                      ,[ddd2] as Ddd2
                      ,[numtel2] as NumTel2
                      ,[estado] as Estado
                      ,[tempoempresaanos] as TempoEmpresaAnos
                      ,[tempoempresaanosint] as TempoEmpresaAnosInt
                      ,[tempoempresamesesint] as TempoEmpresaMesesInt
                      ,[tempoempresatexto] as TempoEmpresaTeexto
                  FROM [DW_IMETAME_NOVA_OS].[dbo].VW_FUSION_GP_COLABORADOR (nolock) COLA
                  WHERE empresa ='01' and status = '1-Ativo'
               		 order by Nome";
                #endregion CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS


                var lista = (await this.conn.QueryAsync<ColaboradorModel>(sql));

                int totalCount = lista.Count();

                lista = lista.Skip(offset).Take(pageSize);

                IList<ColaboradorModel> listColaboradores = lista.Skip(offset).Take(pageSize).ToList();

                PaginatedResponse<ColaboradorModel> paginatedResponse = new PaginatedResponse<ColaboradorModel>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = listColaboradores
                };
                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                #region CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS
                conn.Open();
                var sql = @"SELECT distinct [empresa] as Empresa
                      ,[numcad] as NumCad
                      ,[numcracha] as NumCracha
                      ,[status] as Status
                      ,[nomefuncionario] as Nome
                      ,[cpf] as Cpf
                      ,[funcaoatual] as FuncaoAtual
                      ,[funcaoinicial] as FuncaoInicial
                      ,[dataafastamento] as DataAfastamento
                      ,[dataadmissao] as DataAdmissao
                      ,[datanascimento] as DataNascimento
                      ,[equipe] as Equipe
                      ,[perfil] as Perfil
                      ,[endereco] as Endereco
                      ,[numero] as Numero
                      ,[bairro] as Bairro
                      ,[cidade] as Cidade
                      ,[cep] as Cep
                      ,[ddd] as Ddd
                      ,[numtel] as NumTel
                      ,[ddd2] as Ddd2
                      ,[numtel2] as NumTel2
                      ,[estado] as Estado
                      ,[tempoempresaanos] as TempoEmpresaAnos
                      ,[tempoempresaanosint] as TempoEmpresaAnosInt
                      ,[tempoempresamesesint] as TempoEmpresaMesesInt
                      ,[tempoempresatexto] as TempoEmpresaTeexto
                  FROM [DW_IMETAME_NOVA_OS].[dbo].VW_FUSION_GP_COLABORADOR (nolock) COLA
                  WHERE empresa ='01' and status = '1-Ativo'
               		 order by Nome";
                #endregion CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS


                var lista = (await this.conn.QueryAsync<ColaboradorModel>(sql));

                return Ok(lista);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("{matricula}")]
        public async Task<IActionResult> GetDocumentosProtheus(string matricula, CancellationToken cancellationToken)
        {
            try
            {
                conn.Open();
                var sql = @"SELECT 
                            UZJ.R_E_C_N_O_ AS Id,
                            UZI.UZI_DESC AS DescArquivo,
                            UZJ.UZJ_VENC AS DtVencimento,
                            SRA.RA_NOME AS NomeColaborador,
                            UZJ.UZJ_DOC AS NomeArquivo,
                            UZJ.UZJ_IMG AS Bytes
                        FROM 
                            DADOSADV..UZJ010 UZJ
                            INNER JOIN DADOSADV..UZI010 UZI 
    	                        ON UZI.UZI_FILIAL = UZJ.UZJ_FILIAL 
    	                        AND UZI.UZI_CODIGO = UZJ.UZJ_CODTDO           
                                AND UZI.D_E_L_E_T_ = ''
                            INNER JOIN DADOSADV..SRA010 SRA ON SRA.RA_FILIAL = UZJ.UZJ_FILIAL  
                                AND SRA.RA_MAT = UZJ.UZJ_MAT 
                                AND SRA.D_E_L_E_T_ = ''
                        WHERE 
                            UZJ.UZJ_FILIAL = ''
                            AND UZJ.UZJ_MAT = @Matricula 
                            AND UZJ.UZJ_CODTDO <> '01' 
                            AND UZJ.UZJ_SEQ = (
                                SELECT MAX(UZJ_SEQ) 
                                FROM DADOSADV..UZJ010 
                                WHERE UZJ_FILIAL = UZJ.UZJ_FILIAL 
                                    AND UZJ_MAT = UZJ.UZJ_MAT 
                                    AND UZJ_CODTDO = UZJ.UZJ_CODTDO 
                                    AND D_E_L_E_T_ = ''
                            )
                            AND UZJ.D_E_L_E_T_ = ''";

                var documentos = (await conn.QueryAsync<DocumentoxColaboradorModel>(sql, new { Matricula = matricula })).ToList();

                // Convertendo bytes para Base64
                documentos.ForEach(doc =>
                {
                    if (doc.Bytes != null)
                    {
                        doc.Base64 = $"data:image/png;base64,{Convert.ToBase64String(doc.Bytes)}";
                    }
                });

                return Ok(documentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }


        #region FUNÇÕES CRUD - MATHEUS MONFREIDES FARTEC SISTEMAS
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Colaborador model, CancellationToken cancellationToken)
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
                await _repColaborador.SaveAsync(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Colaborador model, CancellationToken cancellationToken)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                await _repColaborador.UpdateAsync(model);


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
                Colaborador Colaborador = await _repColaborador.SelectAsync(id) ?? throw new Exception("Erro ao apagar o Colaborador com o id fornecido.");

                await _repColaborador.DeleteAsync(Colaborador);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Colaborador excluido com sucesso"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
