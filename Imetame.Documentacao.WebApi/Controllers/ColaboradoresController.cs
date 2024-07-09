using Dapper;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Imetame.Documentacao.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ColaboradoresController : ControllerBase
    {
        private readonly IColaboradorRepository _repository;
        private readonly IBaseRepository<Domain.Entities.Processamento> _repProcessamento;
        protected readonly SqlConnection conn;
        private readonly IConfiguration _configuration;


        public ColaboradoresController(IColaboradorRepository repository, IBaseRepository<Domain.Entities.Processamento> repProcessamento, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
            _repProcessamento = repProcessamento;
            conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet("{idProcessamento}")]
        public async Task<IActionResult> GetColaboradores(Guid idProcessamento, CancellationToken cancellationToken)
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

    }
}
