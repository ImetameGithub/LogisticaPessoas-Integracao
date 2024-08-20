using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using System.Text;

namespace Imetame.Documentacao.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class PedidoController : Controller
    {
        private readonly IBaseRepository<Pedido> _repPedido;
        private readonly IConfiguration _configuration;
        public PedidoController(IBaseRepository<Pedido> repPedido, IConfiguration configuration)
        {
            _repPedido = repPedido;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated(int page = 1, int pageSize = 10, string texto = "")
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Parametros necessarios nao informados");

                int offset = (page - 1) * pageSize;

                IQueryable<Pedido> query = _repPedido.SelectContext()
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

            Pedido Pedido = await _repPedido.SelectContext()                                                            
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
    }
}
