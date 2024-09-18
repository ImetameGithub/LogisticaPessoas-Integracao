using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Infra.Data.Mappings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Imetame.Documentacao.Infra.Data.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {

        private readonly IConfiguration _configuration;
        private readonly IProcessamentoRepository _processamentoRepository;
        protected readonly SqlConnection conn;

        public ColaboradorRepository(IConfiguration configuration, IProcessamentoRepository processamentoRepository)
        {
            _configuration = configuration;
            this._processamentoRepository = processamentoRepository;
            conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        private bool _disposed = false;



        ~ColaboradorRepository() =>
             Dispose();

        public void Dispose()
        {
            if (!_disposed)
            {
                conn.Close();
                conn.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

       

       

        public async Task<IEnumerable<Domain.Models.ColaboradorModel>> ListaAsync(Guid idProcessamento, CancellationToken cancellationToken)
        {
            var processamento = await _processamentoRepository.GetByIdAsync(idProcessamento, cancellationToken);
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
                      join ZNB010 (nolock) ZNB ON ZNB.ZNB_MATRIC = COLAB.[numcad] AND ZNB.D_E_L_E_T_='' AND ZNB.ZNB_DTFIM>GETDATE()-30                       
                      WHERE ZNB_OS = @Oss                       
                    order by Nome";
            //WHERE ZNB_OS in ('001701001')";
            var lista = (await this.conn.QueryAsync<ColaboradorModel>(sql, new {Oss= processamento.Oss }));

            return lista;
        }

        public async Task<IEnumerable<Domain.Models.DocumentoModel>> ListaDocumentosAsync(string numEmp, string cracha, CancellationToken cancellationToken)
        {
            var empresa = await GetPrhotheusEmpresa(numEmp);
            if (string.IsNullOrEmpty(empresa)) empresa = "01";


            var sql = @"SELECT 
		                        Id = UZJ.R_E_C_N_O_,
		                        Tipo =RTRIM(LTRIM(UZI.UZI_DESC)), 
		                        Sequencia = RTRIM(LTRIM(ISNULL(UZJ.UZJ_SEQ, ''))), 		
		                        RTRIM(LTRIM(UZJ_DOC)) as Nome
		                        --UZJ_IMG as ImagemDocumento
                        FROM  UZJ{0}0 UZJ
		                        INNER JOIN UZI010 UZI	ON UZI.D_E_L_E_T_ = '' AND UZI.UZI_CODIGO = UZJ.UZJ_CODTDO
		                        INNER JOIN SRA{0}0 SRA ON SRA.D_E_L_E_T_ = ''AND SRA.RA_MAT = UZJ.UZJ_MAT

                        WHERE UZJ.D_E_L_E_T_ = ''
                        AND UZJ.UZJ_CODTDO <> '01'
                        AND CAST(SRA.RA_CHAPA AS INT) = @Cracha
                        --AND SRA.RA_DEMISSA = ''
                        ORDER BY Tipo, Sequencia";

            sql = String.Format(sql, empresa);


            return (await this.conn.QueryAsync<DocumentoModel>(sql, new { Cracha = Convert.ToInt32(cracha) }));
        }

        public async Task<string> GetPrhotheusEmpresa(string numEmp)
        {

            var sql = @"select M0_CODIGO from dbo.FiliaisMicrosiga
                         where M0_CODIGO NOT IN ('99','11') AND M0_CODFIL = '01' AND SUBSTRING(M0_CODIGO,1,1) <> 'A' 
                                AND (SELECT MIN(numemp) FROM VETORH..r030fil WHERE CAST(numcgc AS bigint) = CAST(M0_CGC AS BIGINT) AND codfil = 1) = @NumEmp
                      ";
            return (await this.conn.QueryAsync<Domain.Entities.Empresa>(sql, new { NumEmp = numEmp })).FirstOrDefault()?.M0_CODIGO;

        }

        public async Task<Imagem> ObterDocumentoAsync(string numEmp, long id, CancellationToken cancellationToken)
        {
            var empresa = await GetPrhotheusEmpresa(numEmp);
            if (string.IsNullOrEmpty(empresa)) empresa = "01";


            var sql = @"SELECT 
		                        Id = UZJ.R_E_C_N_O_,		
                                UZJ_DOC as Nome,
		                        UZJ_IMG as Bytes
                        FROM	UZJ{0}0 UZJ
		                        INNER JOIN UZI010 UZI	ON UZI.D_E_L_E_T_ = '' AND UZI.UZI_CODIGO = UZJ.UZJ_CODTDO
		                        INNER JOIN SRA{0}0 SRA ON SRA.D_E_L_E_T_ = ''AND SRA.RA_MAT = UZJ.UZJ_MAT

                        WHERE   UZJ.D_E_L_E_T_ = ''
                                AND UZJ.UZJ_CODTDO <> '01'
                                AND UZJ.R_E_C_N_O_ = @Id";

            sql = String.Format(sql, empresa);


            return (await this.conn.QueryAsync<Imagem>(sql, new { Id = id })).FirstOrDefault();
        }
    }

}
