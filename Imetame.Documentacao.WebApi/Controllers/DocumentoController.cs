using Dapper;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using System.Text;
using System.Text.Json;

namespace Imetame.Documentacao.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class DocumentoController : Controller
    {
        private readonly IBaseRepository<Documento> _repDocumento;
        private readonly DestraController _destraController;
        private readonly IConfiguration _configuration;
        protected readonly SqlConnection conn;
        public DocumentoController(IBaseRepository<Documento> repDocumento, DestraController destraController, IConfiguration configuration)
        {
            _repDocumento = repDocumento;
            _destraController = destraController;
            _configuration = configuration;
            conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated(int page = 1, int pageSize = 10, string texto = "")
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Parametros necessarios nao informados");

                int offset = (page - 1) * pageSize;

                IQueryable<Documento> query = _repDocumento.SelectContext()
                                                             .OrderBy(x => x.Descricao);
                if (!string.IsNullOrEmpty(texto))
                    query = query.Where(q => q.Descricao.Contains(texto));

                int totalCount = await query.CountAsync();

                IList<Documento> registros = query.Skip(offset)
                                                             .Take(pageSize).ToList();

                PaginatedResponse<Documento> paginatedResponse = new PaginatedResponse<Documento>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = registros
                };
                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }


        #region FUN��ES DE APOIO - MATHEUS MONFREIDES FARTEC SISTEMAS

        #region GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                throw new Exception("Parametros necessarios nao informados");

            List<Documento> listaPedido = await _repDocumento.SelectContext()
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

            Documento Documento = await _repDocumento.SelectContext()
                                                            .Where(e => e.Id.Equals(id))
                                                            .FirstAsync();
            return Ok(Documento);
        }
        #endregion GET ALL        


        #endregion

        #region FUN��ES CRUD - MATHEUS MONFREIDES FARTEC SISTEMAS
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Documento model, CancellationToken cancellationToken)
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
                await _repDocumento.SaveAsync(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }
        #endregion
        
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Documento model, CancellationToken cancellationToken)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                await _repDocumento.UpdateAsync(model);


                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }
        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id,CancellationToken cancellationToken)
        {
            try
            {
                Documento Documento = await _repDocumento.SelectAsync(id) ?? throw new Exception("Erro ao apagar o Pedido com o id fornecido.");

                await _repDocumento.DeleteAsync(Documento);

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
        

        #region GET_ATIVIDADES_DESTRA
        [HttpGet()]
        public async Task<IActionResult> GetDocumentosDestra(CancellationToken cancellationToken)
        {
            try
            {
                var jsonResponse = await _destraController.GetDocumentos();

                List<DocumentosDestra> listDocumentos = new List<DocumentosDestra>();

                ListaDocumentosDestraModel listaModel = JsonSerializer.Deserialize<ListaDocumentosDestraModel>(jsonResponse);
                //var listaAux = JsonSerializer.Deserialize<string[]>(jsonResponse);

                //return Ok(listaAux);

                if (listaModel != null)
                {
                    return Ok(listaModel.LISTA);
                }
                else
                {
                    return BadRequest("Api de Documentos Destra vazia");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetDocumentosProtheus(CancellationToken cancellationToken)
        {
            try
            {
                conn.Open();
                var sql = @"SELECT UZI_CODIGO AS codigo,TRIM(UZI_DESC) as nome FROM UZI010 WHERE D_E_L_E_T_ = '';";                
                var documentos = (await this.conn.QueryAsync<DocumentosProtheus>(sql));
               
                return Ok(documentos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        

    }
}