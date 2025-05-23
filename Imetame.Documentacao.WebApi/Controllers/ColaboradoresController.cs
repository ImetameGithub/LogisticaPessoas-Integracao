﻿using Dapper;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office.Word;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;
using Imetame.Documentacao.Domain.Dto;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Pedido = Imetame.Documentacao.Domain.Entities.Pedido;

namespace Imetame.Documentacao.WebApi.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly IColaboradorRepository _repository;
        private readonly IBaseRepository<Domain.Entities.Processamento> _repProcessamento;
        private readonly IBaseRepository<Domain.Entities.DocumentoxColaborador> _repDocxColaborador;
        private readonly IBaseRepository<Domain.Entities.Documento> _repDocumento;
        private readonly IBaseRepository<Domain.Entities.DocumentoXProtheus> _repDocumentoProtheus;
        private readonly IBaseRepository<Domain.Entities.Colaborador> _repColaborador;
        private readonly IBaseRepository<ColaboradorxAtividade> _repColaboradorxAtividade;
        private readonly IBaseRepository<ColaboradorxPedido> _repColaboradorxPedido;
        private readonly IBaseRepository<Pedido> _repPedido;
        private readonly DestraController _destraController;
        protected readonly SqlConnection conn;
        private readonly IConfiguration _configuration;



        public ColaboradoresController(IColaboradorRepository repository, IBaseRepository<Domain.Entities.Processamento> repProcessamento,
            IConfiguration configuration, IBaseRepository<Domain.Entities.Colaborador> repColaborador,
            IBaseRepository<ColaboradorxAtividade> repColaboradorxAtividade, DestraController destraController, IBaseRepository<Documento> repDocumento, IBaseRepository<DocumentoXProtheus> repDocumentoProtheus,

            IBaseRepository<DocumentoxColaborador> repDocxColaborador, IBaseRepository<ColaboradorxPedido> repColaboradorxPedido, IBaseRepository<Pedido> repPedido)
        {
            _repository = repository;
            _configuration = configuration;
            _repProcessamento = repProcessamento;
            conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            _repColaborador = repColaborador;
            _repColaboradorxAtividade = repColaboradorxAtividade;
            _destraController = destraController;
            _repDocumento = repDocumento;
            _repDocumentoProtheus = repDocumentoProtheus;
            _repDocxColaborador = repDocxColaborador;
            _repColaboradorxPedido = repColaboradorxPedido;
            _repPedido = repPedido;
        }

        #region FUNÇÕES DE APOIO 

        [HttpPost]
        public async Task<IActionResult> RelacionarColaboradorxAtividade([FromBody] ColaboradorxAtividadeModel model, CancellationToken cancellationToken)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                List<Colaborador> colaboradores = new List<Colaborador>();



                foreach (ColaboradorProtheusModel item in model.ListColaborador)
                {
                    ColaboradorDestra colaboradorDestra = new ColaboradorDestra()
                    {
                        nome = item.NOME,
                        nascto = $"{item.NASCIMENTO.Substring(0, 4)}-{item.NASCIMENTO.Substring(4, 2)}-{item.NASCIMENTO.Substring(6, 2)}",
                        cpf = item.CPF,
                        rg = item.RG,
                        passaporte = "",
                        passaporteValidade = "",
                        cnh = "",
                        creaUF = "",
                        idCidade = 2930774,
                        cnpj = "31790710000609",
                        funcao = item.NOME_FUNCAO,
                        idVinculo = 1,
                        dataAdmissao = $"{item.DATA_ADIMISSAO.Substring(0, 4)}-{item.DATA_ADIMISSAO.Substring(4, 2)}-{item.DATA_ADIMISSAO.Substring(6, 2)}",
                        isTemporario = "",
                        contratoDias = 1,
                        contratoFim = "",
                        salarioTipo = "MES",
                    };


                    var jsonResponse = await _destraController.AddColaborador(colaboradorDestra) as OkObjectResult;

                    Colaborador colaborador = new Colaborador()
                    {
                        Id = new Guid(),
                        Matricula = item.MATRICULA.Remove(0, 1), // É DESSA FORMA POIS O PROTHEUS É COM ZERO NO INICIO MAS A CONSULTA TIRA PARA A FUNÇÃO EnviarColaboradorDestra
                        Cpf = item.CPF,
                        Rg = item.RG,
                        Nascimento = item.NASCIMENTO,
                        DataAdmissao = item.DATA_ADIMISSAO,
                        Cracha = item.CRACHA,
                        Nome = item.NOME,
                        Codigo_Funcao = item.CODIGO_FUNCAO,
                        Nome_Funcao = item.NOME_FUNCAO,
                        SincronizadoDestra = true,
                        Codigo_Equipe = item.CODIGO_EQUIPE,
                        Nome_Equipe = item.NOME_EQUIPE,
                        Codigo_Disciplina = item.CODIGO_DISCIPLINA,
                        Nome_Disciplina = item.NOME_DISCIPLINA,
                        Perfil = item.PERFIL,
                        Codigo_OS = item.CODIGO_OS,
                        MudaFuncao = "N",// DEFINIR COMO A CONSULTA VAI TRAZER
                        Nome_OS = item.NOME_OS
                    };

                    colaboradores.Add(colaborador);
                }

                await _repColaborador.InsertRangeAsync(colaboradores);

                List<ColaboradorxAtividade> colaboradorxAtividades = new List<ColaboradorxAtividade>();

                // Cria as relações entre colaboradores e atividades
                foreach (var colaborador in colaboradores)
                {
                    foreach (var atividadeId in model.ListAtividade)
                    {
                        ColaboradorxAtividade relacao = new ColaboradorxAtividade()
                        {
                            Id = new Guid(),
                            CXA_IDCOLABORADOR = colaborador.Id, // Assumindo que o Id do colaborador seja do tipo Guid e esteja gerado
                            CXA_IDATIVIDADE_ESPECIFICA = atividadeId
                        };

                        colaboradorxAtividades.Add(relacao);
                    }
                }

                await _repColaboradorxAtividade.InsertRangeAsync(colaboradorxAtividades);
                //model.Id = new Guid();
                //await _repAtividadeEspecifica.SaveAsync(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        #region FUNÇÕES GET DE TODOS OS TIPOS - Matheus Monfreides

        [HttpGet("{idProcessamento}/{ordemServico}")]
        public async Task<IActionResult> GetColaboradoresPorOs(Guid idProcessamento, string ordemServico)
        {
            try
            {
                //var resp = await _repository.ListaAsync(idProcessamento, cancellationToken);




                Domain.Entities.Processamento? processamento = await _repProcessamento.SelectContext().Where(m => m.Id == idProcessamento).FirstOrDefaultAsync();

#if DEBUG
                var sql = @"WITH CountDocumentos as
							(	SELECT
							    	SUBSTRING(SRA.RA_MAT,2,5) RA_MAT,
							    	COUNT(*) as contagem
								FROM
									SRA010 SRA
								INNER JOIN UZJ010 UZJ 
							        ON
									UZJ.UZJ_FILIAL = SRA.RA_FILIAL
									AND UZJ.UZJ_MAT = SRA.RA_MAT
									AND UZJ.D_E_L_E_T_ = ''
								INNER JOIN UZI010 UZI 
							        ON
									UZI.UZI_FILIAL = UZJ.UZJ_FILIAL
									AND UZI.UZI_CODIGO = UZJ.UZJ_CODTDO
									AND UZI.D_E_L_E_T_ = ''
									AND UZJ.UZJ_CODTDO <> '01'
								WHERE
									RA_FILIAL = ''
									AND SRA.RA_SITFOLH = ''
									AND SRA.D_E_L_E_T_ = ''
                                    AND SRA.RA_DEMISSA = ''
								GROUP BY SRA.RA_MAT)
							SELECT
								distinct [empresa] as Empresa,
								CD.contagem AS CountDocumento,
								[numcad] as NumCad,
								[numcracha] as NumCracha,
								[status] as Status,
								[nomefuncionario] as Nome,
								[cpf] as Cpf,
								[funcaoatual] as FuncaoAtual,
								[funcaoinicial] as FuncaoInicial,
								[dataafastamento] as DataAfastamento,
								[dataadmissao] as DataAdmissao,
								[datanascimento] as DataNascimento,
								[equipe] as Equipe,
								[perfil] as Perfil,
								[endereco] as Endereco,
								[numero] as Numero,
								[bairro] as Bairro,
								[cidade] as Cidade,
								[cep] as Cep,
								[ddd] as Ddd,
								[numtel] as NumTel,
								[ddd2] as Ddd2,
								[numtel2] as NumTel2,
								[estado] as Estado,
								[tempoempresaanos] as TempoEmpresaAnos,
								[tempoempresaanosint] as TempoEmpresaAnosInt,
								[tempoempresamesesint] as TempoEmpresaMesesInt,
								[tempoempresatexto] as TempoEmpresaTeexto
							FROM
								VW_FUSION_GP_COLABORADOR (nolock) COLAB
							join ZNB010 (nolock) ZNB ON
								ZNB.ZNB_MATRIC = COLAB.[numcad]
								AND ZNB.D_E_L_E_T_ = ''
								AND ZNB.ZNB_DTFIM>GETDATE()-30
							JOIN CountDocumentos CD ON CD.RA_MAT = [numcad]
							WHERE
								ZNB_OS = @Oss
								AND GETDATE() BETWEEN ZNB.ZNB_DTINI AND ZNB.ZNB_DTFIM
							order by
								Nome";
#else
    var sql = @"WITH CountDocumentos as
							(	SELECT
							    	SUBSTRING(SRA.RA_MAT,2,5) RA_MAT,
							    	COUNT(*) as contagem
								FROM
									SRA010 SRA
								INNER JOIN UZJ010 UZJ 
							        ON
									UZJ.UZJ_FILIAL = SRA.RA_FILIAL
									AND UZJ.UZJ_MAT = SRA.RA_MAT
									AND UZJ.D_E_L_E_T_ = ''
								INNER JOIN UZI010 UZI 
							        ON
									UZI.UZI_FILIAL = UZJ.UZJ_FILIAL
									AND UZI.UZI_CODIGO = UZJ.UZJ_CODTDO
									AND UZI.D_E_L_E_T_ = ''
									AND UZJ.UZJ_CODTDO <> '01'
								WHERE
									RA_FILIAL = ''
									AND SRA.RA_SITFOLH = ''
									AND SRA.D_E_L_E_T_ = ''
                                    AND SRA.RA_DEMISSA = ''
								GROUP BY SRA.RA_MAT)
							SELECT
								distinct [empresa] as Empresa,
								CD.contagem AS CountDocumento,
								[numcad] as NumCad,
								[numcracha] as NumCracha,
								[status] as Status,
								[nomefuncionario] as Nome,
								[cpf] as Cpf,
								[funcaoatual] as FuncaoAtual,
								[funcaoinicial] as FuncaoInicial,
								[dataafastamento] as DataAfastamento,
								[dataadmissao] as DataAdmissao,
								[datanascimento] as DataNascimento,
								[equipe] as Equipe,
								[perfil] as Perfil,
								[endereco] as Endereco,
								[numero] as Numero,
								[bairro] as Bairro,
								[cidade] as Cidade,
								[cep] as Cep,
								[ddd] as Ddd,
								[numtel] as NumTel,
								[ddd2] as Ddd2,
								[numtel2] as NumTel2,
								[estado] as Estado,
								[tempoempresaanos] as TempoEmpresaAnos,
								[tempoempresaanosint] as TempoEmpresaAnosInt,
								[tempoempresamesesint] as TempoEmpresaMesesInt,
								[tempoempresatexto] as TempoEmpresaTeexto
							FROM
								DW_IMETAME_NOVA_OS..VW_FUSION_GP_COLABORADOR (nolock) COLAB
							join ZNB010 (nolock) ZNB ON
								ZNB.ZNB_MATRIC = COLAB.[numcad]
								AND ZNB.D_E_L_E_T_ = ''
								AND ZNB.ZNB_DTFIM>GETDATE()-30
							JOIN CountDocumentos CD ON CD.RA_MAT = [numcad]
							WHERE
								ZNB_OS = @Oss
								AND GETDATE() BETWEEN ZNB.ZNB_DTINI AND ZNB.ZNB_DTFIM
							order by
								Nome";
#endif


                #endregion CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS

                var lista = (await this.conn.QueryAsync<ColaboradorModel>(sql, new { Oss = processamento.Oss }));
                List<DocumentoStatus> docsStatusList = await _destraController.GetStatusDocumentos(ordemServico);
                foreach (var item in lista)
                {
                    Colaborador? colaboradorCad = await _repColaborador.SelectContext()
                         .AsNoTracking()
                         .Where(m => m.Matricula == item.NumCad)
                         .Include(m => m.ColaboradorxAtividade)
                             .ThenInclude(m => m.AtividadeEspecifica)
                         .FirstOrDefaultAsync();

                    item.SincronizadoDestra = false;
                    if (colaboradorCad is not null)
                    {
                        // SE O COLABORADOR NÃO FOR SINCRONIZADO PARA DESTRA NÃO TEM COLABORADOR CADASTRO NO SISTEMA

                        item.IsAssociado = await _repColaboradorxPedido.SelectContext().AsNoTracking()
                                            .AnyAsync(cp => cp.CXP_IDCOLABORADOR == colaboradorCad.Id
                                                        && cp.CXP_IDPEDIDO == processamento.IdPedido
                                                         && cp.CXP_NUMEROOS == processamento.Oss);
                    }

                    string json = await _destraController.GetColaborador(item.Cpf.PadLeft(11, '0'));
                    ColaboradorDestraApiModel colaboradorDestra = JsonConvert.DeserializeObject<ColaboradorDestraApiModel>(json);
                    if (colaboradorDestra.DADOS.Count != 0)
                    {
                        item.StatusDestra = (Int32)colaboradorDestra.DADOS[0].status;
                        item.SincronizadoDestra = true;
                    }
                    else
                    {
                        item.SincronizadoDestra = false;
                    }


                    item.DocumentoStatus = docsStatusList.Where(m => m.Cpf == item.Cpf).ToList();
                    foreach(var doc in item.DocumentoStatus)
                    {
                        var docRelacao = _repDocumentoProtheus.SelectContext().Include(m => m.Documento).Where(m => m.Documento.IdDestra == doc.Codigo.ToString()).First();

                        doc.CodProtheus = docRelacao.IdProtheus;

                    }
                    item.DocumentoPendencia = item.DocumentoStatus.Any();
                    if (item.DocumentoPendencia)
                        item.StatusDestra = -1;
                }
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

                // CONSULTA NÃO OTIMIZADA ESTREMAMENTE LENTA
                //IQueryable<Colaborador> query = _repColaborador.SelectContext()
                //											 .AsNoTracking()
                //											 .OrderBy(x => x.Nome)
                //											 .Include(x => x.DocumentosxColaborador)
                //											 .Include(m => m.ColaboradorxAtividade)
                //												.ThenInclude(m => m.AtividadeEspecifica);

                IQueryable<Colaborador> query = _repColaborador.SelectContext()
                                                             .AsNoTracking()
                                                             .OrderBy(x => x.Nome)
                                                             .Include(m => m.ColaboradorxAtividade)
                                                                .ThenInclude(m => m.AtividadeEspecifica);

                if (!string.IsNullOrEmpty(texto))
                    query = query.Where(q => q.Nome.Contains(texto) || q.Matricula.Contains(texto));

                int totalCount = await query.CountAsync();

                IList<Colaborador> listColaboradors = await query.Skip(offset)
                                                             .Take(pageSize).ToListAsync();

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
                var sql = @"SELECT SRA.RA_MAT AS MATRICULA,
	                               SRA.RA_CRACHA AS CRACHA,
	                               SRA.RA_NOME AS NOME,
	                               SRJ.RJ_FUNCAO AS CODIGO_FUNCAO,
	                               SRJ.RJ_DESC AS NOME_FUNCAO, 
	                               UZD.UZD_CODIGO AS CODIGO_EQUIPE,
	                               UZD.UZD_SIGLA AS NOME_EQUIPE,
	                               ZA3.ZA3_DESC AS PERFIL,
	                               ZNB.ZNB_OS AS CODIGO_OS,
	                               ZNB.ZNB_NOMEOS AS NOME_OS
                            FROM SRA010 SRA (NOLOCK)
                            LEFT JOIN SRJ010 SRJ (NOLOCK) --FUNÇOES
	                            ON SRJ.RJ_FILIAL = SRA.RA_FILIAL 
	                            AND SRJ.RJ_FUNCAO = SRA.RA_CODFUNC 
	                            AND SRJ.D_E_L_E_T_ = '' 
                            LEFT JOIN UZD010 UZD (NOLOCK) --EQUIPE
	                            ON UZD.UZD_FILIAL = SRA.RA_FILIAL 	
	                            AND UZD.UZD_CODIGO = SRA.RA_YEQATUA	
	                            AND UZD.D_E_L_E_T_ = ''
                            LEFT JOIN ZA3010 AS ZA3 (NOLOCK) --PERFIL 
	                            ON ZA3.ZA3_FILIAL = UZD.UZD_FILIAL  
	                            AND ZA3.ZA3_CODIGO = UZD_CODPER 
	                            AND ZA3.D_E_L_E_T_ = ''
                            LEFT JOIN ZNB010 ZNB (NOLOCK) --OS
	                            ON ZNB.ZNB_MATRIC = SRA.RA_MAT 
	                            AND ZNB.D_E_L_E_T_='' 	
                            WHERE SRA.RA_SITFOLH = ''
	                            AND GETDATE() BETWEEN ZNB.ZNB_DTINI AND ZNB.ZNB_DTFIM
	                            AND SRA.D_E_L_E_T_ = ''";
                #endregion CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS


                var lista = (await this.conn.QueryAsync<ColaboradorProtheusModel>(sql));

                int totalCount = lista.Count();

                lista = lista.Skip(offset).Take(pageSize);

                IList<ColaboradorProtheusModel> listColaboradores = lista.Skip(offset).Take(pageSize).ToList();

                PaginatedResponse<ColaboradorProtheusModel> paginatedResponse = new PaginatedResponse<ColaboradorProtheusModel>
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
                var sql = @"SELECT SRA.RA_MAT AS MATRICULA,
                                       SRA.RA_CRACHA AS CRACHA,
                                       SRA.RA_NOME AS NOME,
				                       SRA.RA_NASC AS NASCIMENTO,
				                       SRA.RA_CIC AS CPF,
				                       SRA.RA_RG AS RG,
				                       SRA.RA_ADMISSA AS DATA_ADIMISSAO,
                                       SRJ.RJ_FUNCAO AS CODIGO_FUNCAO,
                                       SRJ.RJ_DESC AS NOME_FUNCAO, 
                                       UZD.UZD_CODIGO AS CODIGO_EQUIPE,
                                       UZD.UZD_SIGLA AS NOME_EQUIPE,
                                       ZA3.ZA3_DESC AS PERFIL,
                                       ZNB.ZNB_OS AS CODIGO_OS,
                                       ZNB.ZNB_NOMEOS AS NOME_OS,
                                       ZG0.ZG0_IDDECI AS CODIGO_DISCIPLINA,
                                       ZG0.ZG0_DEDECI AS NOME_DISCIPLINA
                                FROM SRA010 SRA (NOLOCK)
                                LEFT JOIN SRJ010 SRJ (NOLOCK) --FUNÇOES
                                    ON SRJ.RJ_FILIAL = SRA.RA_FILIAL 
                                    AND SRJ.RJ_FUNCAO = SRA.RA_CODFUNC 
                                    AND SRJ.D_E_L_E_T_ = '' 
                                LEFT JOIN UZD010 UZD (NOLOCK) --EQUIPE
                                    ON UZD.UZD_FILIAL = SRA.RA_FILIAL 	
                                    AND UZD.UZD_CODIGO = SRA.RA_YEQATUA	
                                    AND UZD.D_E_L_E_T_ = ''
                                LEFT JOIN ZA3010 AS ZA3 (NOLOCK) --PERFIL 
                                    ON ZA3.ZA3_FILIAL = UZD.UZD_FILIAL  
                                    AND ZA3.ZA3_CODIGO = UZD_CODPER 
                                    AND ZA3.D_E_L_E_T_ = ''
                                LEFT JOIN ZNB010 ZNB (NOLOCK) --OS
                                    ON ZNB.ZNB_MATRIC = SRA.RA_MAT 
                                    AND ZNB.D_E_L_E_T_=''
                                LEFT JOIN ZG0010 ZG0 (NOLOCK) 
                                    ON (TRY_CAST(ZG0.ZG0_NUMCAD AS INTEGER) = TRY_CAST(RA_YNUMCAD AS INTEGER) 
                                    AND ZG0.ZG0_NUMEMP = SRA.RA_YNUMEMP 
                                    AND ZG0.ZG0_TIPCOL = SRA.RA_YTIPCOL AND ZG0.D_E_L_E_T_ <> '*')
                                WHERE SRA.RA_SITFOLH = ''
                                    AND GETDATE() BETWEEN ZNB.ZNB_DTINI AND ZNB.ZNB_DTFIM
                                    AND SRA.D_E_L_E_T_ = ''
                                ORDER BY NOME";
                #endregion CONSULTA SQL - MATHEUS MONFREIDES FARTEC SISTEMAS


                var lista = (await this.conn.QueryAsync<ColaboradorProtheusModel>(sql));
                return Ok(lista);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet("{matricula}")]
        private async Task<List<DocumentoxColaboradorModel>> ConsultaDocsProtheus(string matricula)
        {
            try
            {
                using (var conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"))) // Substitua connectionString pela sua string de conexão
                {
                    conn.Open();
                    var sql = @"SELECT 
                        UZI.UZI_CODIGO AS Codigo,
                        SRA.RA_NOME AS NomeColaborador,
                        SRA.RA_MAT AS Matricula,
                        UZI.UZI_DESC AS DescArquivo,
                        UZJ.UZJ_VENC AS DtVencimento,
                        UZJ.UZJ_DOC AS NomeArquivo,
                        UZJ.R_E_C_N_O_ AS Recno,
                        UZJ.UZJ_SEQ AS Sequencia,     
                        UZJ.UZJ_CODTDO AS IdTipoDocumento,
                        UZI.UZI_DESC AS TipoDocumento
                          FROM SRA010 SRA
                          INNER JOIN UZJ010 UZJ 
                                  ON  UZJ.UZJ_FILIAL = SRA.RA_FILIAL
                                  AND UZJ.UZJ_MAT = SRA.RA_MAT
                                  AND UZJ.D_E_L_E_T_ = ''
                          INNER JOIN UZI010 UZI 
                                  ON UZI.UZI_FILIAL = UZJ.UZJ_FILIAL 
                                  AND UZI.UZI_CODIGO = UZJ.UZJ_CODTDO           
                                  AND UZI.D_E_L_E_T_ = ''
                                  AND UZJ.UZJ_CODTDO <> '01'  
                          WHERE RA_FILIAL = '' 
                              AND RA_MAT = @Matricula
                              AND SRA.D_E_L_E_T_  = ''";

                    List<DocumentoxColaboradorModel> documentos = (await conn.QueryAsync<DocumentoxColaboradorModel>(sql, new { Matricula = matricula })).ToList();

                    foreach (DocumentoxColaboradorModel item in documentos.Where(m => m.Vencido != true || m.Vencer != true))
                    {
                        bool docRelacao = await _repDocxColaborador.SelectContext().AsNoTracking()
                            .Include(m => m.Colaborador)
                            .Where(m => m.DXC_CODPROTHEUS == item.Codigo && m.Colaborador.Matricula == matricula.Remove(0, 1)).AnyAsync();

                        if (docRelacao)
                        {
                            item.SincronizadoDestra = true;
                        }

                        bool relacionadoDestra = await _repDocumento.SelectContext()
                                                                    .AsNoTracking()
                                                                    .Include(x => x.DocumentoXProtheus)
                                                                    .Where(m => m.DocumentoXProtheus!.Select(x => x.IdProtheus).Contains(item.Codigo))
                                                                    .AnyAsync();

                        if (relacionadoDestra)
                        {
                            item.RelacionadoDestra = true;
                        }
                    }

                    return documentos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("{matricula}/{docsStatusListJson}")]
        public async Task<IActionResult> GetDocumentosProtheus(string matricula, string docsStatusListJson)
        {
            try
            {
                List<DocumentoStatus> docsStatusList = JsonConvert.DeserializeObject<List<DocumentoStatus>>(docsStatusListJson);

                List<DocumentoxColaboradorModel> documentos = await ConsultaDocsProtheus(matricula);                

                Colaborador? colaboradorCad = await _repColaborador.SelectContext().AsNoTracking()
                   .Where(m => m.Matricula == matricula.Remove(0, 1))
                   .Include(m => m.ColaboradorxAtividade)
                       .ThenInclude(m => m.AtividadeEspecifica)
                   .FirstOrDefaultAsync();

                if (colaboradorCad is not null)
                {
                    var nomeAtividades = colaboradorCad.ColaboradorxAtividade.Select(m => m.AtividadeEspecifica.Descricao).ToList();
                    string listaDeNomes = string.Join(", ", nomeAtividades);

                    documentos[0].NomeColaborador = documentos[0].NomeColaborador + " - " + listaDeNomes;
                }

                DateTime dataAtual = DateTime.Now;

                foreach (DocumentoxColaboradorModel vencidos in documentos.Where(m => m.DtVencimento.Trim() != ""))
                {
                    //DateTime dtVencimento = Convert.ToDateTime(item.DtVencimento);
                    vencidos.DtVencimentoFormatada = DateTime.ParseExact(vencidos.DtVencimento, "yyyyMMdd", CultureInfo.InvariantCulture);
                    vencidos.DiasVencer = (vencidos.DtVencimentoFormatada - dataAtual).Days;

                    // Verificar se o documento está prestes a vencer em 10 dias - Matheus Monfreides
                    if (vencidos.DtVencimentoFormatada <= dataAtual.AddDays(10) && vencidos.DtVencimentoFormatada > dataAtual)
                    {
                        vencidos.Vencer = true;
                    }

                    // Verificar se o documento já está vencido - Matheus Monfreides
                    if (vencidos.DtVencimentoFormatada <= dataAtual)
                    {
                        vencidos.Vencido = true;
                    }
                }

                foreach (var documento in documentos.Where(doc => docsStatusList.Any(status => status.CodProtheus == doc.Codigo)))
                {
                    // Filtra os elementos de docsStatusList que correspondem ao documento atual
                    documento.DocumentoStatus = docsStatusList
                        .Where(status => status.CodProtheus == documento.Codigo)
                        .First();
                }

                return Ok(documentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("{recno}")]
        public async Task<IActionResult> GetImagemProtheus(string recno)
        {
            try
            {
                conn.Open();
                var sql = @"SELECT UZJ_IMG as Bytes FROM UZJ010 WHERE R_E_C_N_O_ =  @Recno";

                var imagens = (await conn.QueryAsync<byte[]>(sql, new { Recno = recno })).ToList();
                conn.Close();
                ImagemProtheus objeto = new ImagemProtheus();
                foreach (byte[] item in imagens)
                {
                    objeto = new ImagemProtheus
                    {
                        Bytes = item,
                        Base64 = $"data:image/png;base64,{Convert.ToBase64String(item)}"
                    };
                }

                return Ok(objeto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        #endregion




        #region FUNÇÕE DE ENVIO PARA DESTRA
        [HttpPost]
        public async Task<IActionResult> EnviarColaboradorDestra([FromBody] EnviarColaboradorDestraDto dto)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                List<Colaborador> colaboradores = new List<Colaborador>();
                Pedido pedido = await _repPedido.SelectContext().Where(m => m.Id == dto.IdPedido).FirstAsync();
                List<Equipe> equipeList = new List<Equipe>();
                List<ColaboradorxPedido> colaboradorxPedidoList = new List<ColaboradorxPedido>();
                List<string> colaboradoresRelacionados = new List<string>();

                // Carregar todos os itens de colaboradores cadastrados no sistema - Matheus Monfreides
                List<Colaborador> itensCadastrados = _repColaborador.SelectContext()
                    .AsNoTracking()
                    .Include(m => m.ColaboradorxAtividade)
                        .ThenInclude(m => m.AtividadeEspecifica)
                    .ToList();

                #region VALIDAR SE COLABORADOR ESTÁ ATRELADO A ALGUMA ATIVIDADE
                List<ColaboradorModel> colaboradoresSemAtividade = dto.ListColaboradores
                   .Where(ei => !itensCadastrados.Any(m => m.Matricula == ei.NumCad))
                   .ToList();

                if (colaboradoresSemAtividade.Any())
                {
                    var nomesColaboradores = colaboradoresSemAtividade.Select(colaborador => colaborador.Nome).ToList();
                    string listaDeNomes = string.Join(", ", nomesColaboradores);
                    if (nomesColaboradores.Count() > 1)
                    {
                        throw new Exception("Os colaboradores " + listaDeNomes + " não estão relacionado a nenhuma atividade específica.");
                    }
                    else
                    {
                        throw new Exception("O colaborador " + listaDeNomes + " não está relacionado a nenhuma atividade específica.");
                    }
                }
                #endregion

                #region VALIDAR SE COLABORADOR JÁ ESTÁ ATRELADO A UM PEDIDO E OS
                List<ColaboradorModel> colaboradoresAssociadosAoPedido = dto.ListColaboradores
                 .Where(m => m.IsAssociado)
                 .ToList();
                //if (colaboradoresAssociadosAoPedido.Any())
                //{
                //	//ANTES ERA CARREGADA UMA MENSAGEM MAS FOI RETIRADA ESSA LOGICA
                //	//List<string> nomesColaboradores = colaboradoresAssociadosAoPedido.Select(colaborador => colaborador.Nome).ToList();
                //	//colaboradoresRelacionados.AddRange(nomesColaboradores);

                //	// Filtra os colaboradores que não estão associados para o próximo passo
                //	dto.ListColaboradores = dto.ListColaboradores
                //		.Where(m => !colaboradoresAssociadosAoPedido.Any(ca => ca.NumCad == m.NumCad))
                //		.ToList();
                //}
                #endregion

                List<Colaborador> listProtheusXlistColaborador = itensCadastrados
                    .Where(m => dto.ListColaboradores.Any(ei => ei.NumCad == m.Matricula))
                    .ToList();

                foreach (Colaborador model in listProtheusXlistColaborador)
                {
                    #region ENVIAR CADASTRO DE COLABORADOR PARA DESTRA - O ENVIO JÁ É REALIZANDO QUANDO EU RELACIONO O COLABORADOR A UMA ATIVIDADE
                    //ColaboradorDestra colaboradorDestra = new ColaboradorDestra()
                    //{
                    //    nome = model.Nome,
                    //    nascto = model.Nascimento,
                    //    cpf = model.Cpf,
                    //    rg = model.Rg,
                    //    passaporte = "",
                    //    passaporteValidade = "",
                    //    cnh = "",
                    //    cnhCategoria = "",
                    //    CnhValidade = "",
                    //    crea = "",
                    //    creaUF = "",
                    //    idCidade = 2930774,
                    //    cnpj = "31790710000609",
                    //    funcao = model.Nome_Funcao,
                    //    idVinculo = 1,
                    //    dataAdmissao = model.DataAdmissao,
                    //    isTemporario = "",
                    //    contratoDias = 1,
                    //    contratoFim = "",
                    //    salarioTipo = "MES",
                    //    salarioValor = 1,

                    //};

                    //var jsonResponse = await _destraController.AddColaborador(colaboradorDestra) as OkObjectResult;
                    #endregion

                    List<int> atividadesEspecificas = model.ColaboradorxAtividade
                                .Select(ca => ca.AtividadeEspecifica.IdDestra)
                                .ToList();

                    Equipe equipe = new Equipe()
                    {
                        cpf = model.Cpf,
                        atividadeEspecifica = atividadesEspecificas
                    };

                    equipeList.Add(equipe);

                    ColaboradorxPedido colabPedido = new ColaboradorxPedido()
                    {
                        CXP_DTINCLUSAO = DateTime.Now,
                        CXP_IDCOLABORADOR = model.Id,
                        CXP_IDPEDIDO = dto.IdPedido,
                        CXP_NUMEROOS = dto.OrdemServico,
                        CXP_USUARIOINCLUSAO = dto.MatriculaUsuario
                    };
                    colaboradorxPedidoList.Add(colabPedido);
                }

                #region MONTAR ENVIO DE PEDIDO PARA DESTRA 
                if (listProtheusXlistColaborador.Any())
                {
                    IncluirPedidoxDireta incluirDiretaPedido = new IncluirPedidoxDireta()
                    {
                        cnpj = "31790710001834",
                        numeroOS = dto.OrdemServico,
                        tipoContrato = "DIR",
                        pedidoCliente = pedido.NumPedido,
                        observacoes = "",
                        contatoOS = "Responsavel pela OS",
                        telefoneOS = "(19)88888-8888",


                        unidades = new List<Unidade>()
                        {
                            new Unidade()
                            {
                                cnpj = "16.404.287/0156-91",
                                gestor = "Gestor Limeira",
                                email = "limeira@suzano.com.br.teste",
                                telefone = "(19)99999-9900"
                            }
                        },

                        equipe = equipeList,
                    };

                    var jsonResponsePedido = await _destraController.AddPedidoxDireta(incluirDiretaPedido) as OkObjectResult;

                    await _repColaboradorxPedido.InsertRangeAsync(colaboradorxPedidoList);
                }
                else
                {
                    throw new Exception("Os colaboradores selecionados já estão associados ao pedido " + pedido.NumPedido + " e a Ordem de Servico " + dto.OrdemServico);
                }



                //ANTES ERA CARREGADA UMA MENSAGEM MAS FOI RETIRADA ESSA LOGICA
                //if (colaboradoresRelacionados.Any())
                //{
                //    string listaDeNomes = string.Join(", ", colaboradoresRelacionados);
                //    if (colaboradoresRelacionados.Count() > 1)
                //    {
                //        throw new Exception("Os colaboradores " + listaDeNomes + " já estão associados ao pedido " + pedido.NumPedido + " e a Ordem de Servico " + dto.OrdemServico);
                //    }
                //    else
                //    {
                //        throw new Exception("O colaborador " + listaDeNomes + " já está associado ao pedido " + pedido.NumPedido + " e a Ordem de Servico " + dto.OrdemServico);
                //    }
                //}
                #endregion

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarDocsArrayDestra([FromBody] List<ColaboradorModel> listColaboradores)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    return BadRequest("Falha ao Salvar Dados.\n" + erro);
                }

                var matriculas = listColaboradores.Select(c => c.NumCad)
                                                  .Distinct()
                                                  .ToList();

                var itensCadastrados = await _repColaborador.SelectContext()
                                                            .AsNoTracking()
                                                            .Where(c => matriculas.Contains(c.Matricula))
                                                            .ToListAsync();

                var colaboradoresSemAtividade = listColaboradores.Where(ei => !itensCadastrados.Any(m => m.Matricula == ei.NumCad)).ToList();
                if (colaboradoresSemAtividade.Any())
                {
                    var nomesColaboradores = string.Join(", ", colaboradoresSemAtividade.Select(c => c.Nome));
                    var plural = colaboradoresSemAtividade.Count > 1 ? "s" : "";
                    return BadRequest($"O colaborador{plural} {nomesColaboradores} não está{plural} relacionado a nenhuma atividade específica.");
                }

                List<string> colaboradoresComDocsVencidos = new List<string>();
                HashSet<string> docsSemRelacao = new HashSet<string>();
                DateTime dataLimite = DateTime.Now.AddDays(10);

                foreach (ColaboradorModel item in listColaboradores)
                {
                    var docsColaborador = await ConsultaDocsProtheus("0" + item.NumCad);
                    ProcessarDocumentos(docsColaborador, item.Nome, dataLimite, colaboradoresComDocsVencidos, docsSemRelacao);
                }

                if (docsSemRelacao.Any())
                {
                    var docsSemRelacaoStr = string.Join(", ", docsSemRelacao);
                    return BadRequest("Os seguintes documentos não possuem nenhuma relação com os documentos da Destra: " + docsSemRelacaoStr);
                }

                if (colaboradoresComDocsVencidos.Any())
                {
                    var colaboradoresComDocsVencidosStr = string.Join(", ", colaboradoresComDocsVencidos);
                    return BadRequest("Os seguintes colaboradores estão com documentos vencidos ou a vencer: " + colaboradoresComDocsVencidosStr);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        private void ProcessarDocumentos(List<DocumentoxColaboradorModel> docsColaborador, string nomeColaborador, DateTime dataLimite, List<string> colaboradoresComDocsVencidos, HashSet<string> docsSemRelacao)
        {
            var validDocs = docsColaborador.Where(m => !string.IsNullOrWhiteSpace(m.DtVencimento)).ToList();
            validDocs.ForEach(doc => doc.DtVencimentoFormatada = DateTime.ParseExact(doc.DtVencimento, "yyyyMMdd", CultureInfo.InvariantCulture));

            var docsVencidos = validDocs.Where(m => m.DtVencimentoFormatada <= dataLimite && m.DtVencimentoFormatada > DateTime.Now || m.DtVencimentoFormatada <= DateTime.Now).ToList();
            docsColaborador = docsColaborador.Except(docsVencidos).ToList();

            List<Task> tarefas = new List<Task>();
            foreach (var doc in docsColaborador)
            {
                bool docRelacao = _repDocumento.SelectContext().Include(x => x.DocumentoXProtheus).AsNoTracking().Any(m => m.DocumentoXProtheus.Select(x => x.IdProtheus).Contains(doc.Codigo));
                if (!docRelacao)
                {
                    docsSemRelacao.Add(doc.DescArquivo);
                }
                else
                {
                    EnviarDocumentoParaDestra(doc).Wait();
                }
            }

            if (docsVencidos.Any())
            {
                colaboradoresComDocsVencidos.Add(nomeColaborador);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EnviarDocumentoParaDestra([FromBody] DocumentoxColaboradorModel documento)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                Documento? docRelacao = await _repDocumento.SelectContext().Include(x => x.DocumentoXProtheus).AsNoTracking().Where(m => m.DocumentoXProtheus!.Select(x => x.IdProtheus).Contains(documento.Codigo)).FirstOrDefaultAsync();

                if (docRelacao is null)
                    throw new Exception("O documento " + documento.DescArquivo + " não possui nenhuma relação com os documentos da Destra");

                Colaborador? colaboradorCadastrado = await _repColaborador.SelectContext().AsNoTracking().Where(m => m.Matricula == documento.Matricula.Remove(0, 1)).FirstOrDefaultAsync();

                if (colaboradorCadastrado is null)
                    throw new Exception("O colaborador " + documento.NomeColaborador + " não está relacionado a nenhuma atividade específica.");

                if (documento.Bytes is null)
                {
                    var response = await GetImagemProtheus(documento.Recno);

                    if (response is OkObjectResult okResult)
                    {
                        ImagemProtheus objeto = okResult.Value as ImagemProtheus;
                        if (objeto != null)
                        {
                            documento.Bytes = objeto.Bytes;
                            documento.Base64 = objeto.Base64;
                        }
                    }
                    else
                    {
                        throw new Exception("Erro ao obter a imagem Protheus.");
                    }
                }

                DocumentoDestra itemDestra = new DocumentoDestra
                {
                    idDocto = docRelacao.IdDestra,
                    cpf = colaboradorCadastrado.Cpf,
                    arquivo = documento.Bytes,
                    validade = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"),
                    pagina = documento.Sequencia,
                };

                var jsonResponse = await _destraController.EnviarDocumentoParaApiDoCliente(itemDestra, documento.DescArquivo.Trim() + ".pdf");
                if (jsonResponse.IsSuccessStatusCode)
                {
                    documento.SincronizadoDestra = true;

                    DocumentoxColaborador item = new DocumentoxColaborador
                    {
                        Id = new Guid(),
                        DXC_CODPROTHEUS = documento.Codigo,
                        DXC_DESCPROTHEUS = documento.DescArquivo,
                        DXC_CODDESTRA = docRelacao.IdDestra,
                        DXC_DESCDESTRA = docRelacao.DescricaoDestra,
                        DXC_IDCOLABORADOR = colaboradorCadastrado.Id,
                        DXC_BASE64 = $"data:image/png;base64,{Convert.ToBase64String(documento.Bytes)}"
                    };

                    await _repDocxColaborador.SaveAsync(item);
                }


                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        #endregion


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

                // ADCIONADO PARA VALIDAR QUANDO HOUVER DOCUMENTOS RELACIONADOS POIS O MESMO ESTÁ DISPARANDO ERROS
                IList<DocumentoxColaborador> docsColaborador = await _repDocxColaborador.SelectContext()
                                                            .Where(x => x.DXC_IDCOLABORADOR.Equals(Colaborador.Id))
                                                            .ToListAsync();

                if (docsColaborador.Count != 0)
                    await _repDocxColaborador.DeleteRangeAsync(docsColaborador);


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



        [HttpPost]
        public async Task<IActionResult> GetDocumentosObrigatorios(List<DocumentoxColaboradorModel> lista)
        {
            try
            {
                List<StatusDocumentoObrigatoriosModel> StatusDocumentoObrigatoriosDTO = new List<StatusDocumentoObrigatoriosModel>();

                // Verifica se a lista está vazia ou não possui elementos
                if (lista == null || lista.Count == 0)
                {
                    throw new ArgumentException("A lista de documentos do colaborador está vazia.");
                }

                Colaborador? colaboradorCadastrado = await _repColaborador.SelectContext().AsNoTracking()
                    .Where(m => m.Matricula == lista[0].Matricula.Remove(0, 1))
                    .Include(m => m.ColaboradorxAtividade)
                        .ThenInclude(m => m.AtividadeEspecifica)
                    .FirstOrDefaultAsync();

                if (colaboradorCadastrado is null)
                    throw new Exception("O colaborador " + lista[0].NomeColaborador + " não está relacionado a nenhuma atividade específica. Por isso não é possivel consultar seus documentos obrigatórios");

                // Armazena todos os documentos que a Destra solicita pelas atividades nas quais o colaborador foi atrelado 
                List<DocumentosDestra> todosOsDocsDestra = new List<DocumentosDestra>();

                // Busca todos os documentos que a Destra obriga ter para as atividades que o colaborador foi atrelado
                foreach (var colaboradorAtividade in colaboradorCadastrado.ColaboradorxAtividade)
                {
                    var jsonResponse = await _destraController.GetDocumentosRequeridos(colaboradorAtividade.AtividadeEspecifica.IdDestra.ToString());

                    ListaDocumentosDestraModel DocsPorAtividade = JsonConvert.DeserializeObject<ListaDocumentosDestraModel>(jsonResponse);

                    todosOsDocsDestra.AddRange(DocsPorAtividade.LISTA);
                }

                // Verifica a relação entre documentos da Destra e documentos do Protheus
                List<Documento> relacaoDestraProtheus = new List<Documento>();

                foreach (var docDestra in todosOsDocsDestra)
                {
                    Documento docRelacao = await _repDocumento.SelectContext().AsNoTracking()
                        .Where(m => m.IdDestra == docDestra.codigo.ToString()).Include(m => m.DocumentoXProtheus) // Ajustado para usar IdDestra
                        .FirstOrDefaultAsync();

                    if (docRelacao == null)
                    {
                        //StatusDocumentoObrigatoriosDTO.Add("O documento " + docDestra.nome + " não possui uma relação com nenhum documento do Protheus");

                        StatusDocumentoObrigatoriosDTO.Add(
                          new StatusDocumentoObrigatoriosModel
                          {
                              DocDestra = docDestra.nome,
                              DocProtheus = "-",
                              Status = "-"
                          }
                        );
                    }
                    else
                    {
                        relacaoDestraProtheus.Add(docRelacao);
                    }
                }

                // Verifica se o colaborador possui todos os documentos obrigatórios
                foreach (var docRelacao in relacaoDestraProtheus)
                {
                    // TODO NECESSARIO VALIDAR
                    foreach (var docProtheus in docRelacao.DocumentoXProtheus!)
                    {
                        bool possuiDocumento = lista.Any(d => docRelacao.DocumentoXProtheus.Select(x => x.IdProtheus).Contains(d.Codigo));
                        if (possuiDocumento)
                        {
                            StatusDocumentoObrigatoriosDTO.Add(
                            new StatusDocumentoObrigatoriosModel
                            {
                                DocDestra = docRelacao.DescricaoDestra,
                                DocProtheus = docProtheus.DescricaoProtheus,
                                Status = "Pendente"
                            }
                          );
                        }
                        else
                        {
                            StatusDocumentoObrigatoriosDTO.Add(
                              new StatusDocumentoObrigatoriosModel
                              {
                                  DocDestra = docRelacao.DescricaoDestra,
                                  DocProtheus = docProtheus.DescricaoProtheus,
                                  Status = "Ok"
                              });

                        }

                    }
                    // ADICIONAR DOCUMENTOS MARCADOS COMO OBRIGATOÓRIO E RELACIONADOS A LISTA ENVIADA
                    IList<Documento> listDocumentos = await _repDocumento.SelectContext()
                                       .AsNoTracking()
                                       .Include(m => m.DocumentoXProtheus)
                                       .Where(x => lista.Select(y => y.DescArquivo).Contains(x.Descricao) && x.Obrigatorio == true)
                                       .ToListAsync();
                    foreach (var doc in listDocumentos)
                    {
                        foreach (var docProtheus in doc.DocumentoXProtheus!)
                        {
                            StatusDocumentoObrigatoriosDTO.Add(
                              new StatusDocumentoObrigatoriosModel
                              {
                                  DocDestra = doc.DescricaoDestra,
                                  DocProtheus = docProtheus.DescricaoProtheus,
                                  Status = "Ok"
                              });
                        }
                    }
                }
                return Ok(StatusDocumentoObrigatoriosDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}

