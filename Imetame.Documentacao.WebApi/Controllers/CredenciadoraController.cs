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
    public class CredenciadoraController : Controller
    {
        private readonly IBaseRepository<Credenciadora> _repCredenciadora;
        private readonly IConfiguration _configuration;
        public CredenciadoraController(IBaseRepository<Credenciadora> repCredenciadora, IConfiguration configuration)
        {
            _repCredenciadora = repCredenciadora;
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

                IQueryable<Credenciadora> query = _repCredenciadora.SelectContext()
                                                             .OrderBy(x => x.Descricao);
                if (!string.IsNullOrEmpty(texto))
                    query = query.Where(q => q.Descricao.Contains(texto));

                int totalCount = await query.CountAsync();

                IList<Credenciadora> listCredenciadoras = query.Skip(offset)
                                                             .Take(pageSize).ToList();

                PaginatedResponse<Credenciadora> paginatedResponse = new PaginatedResponse<Credenciadora>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = listCredenciadoras
                };
                return Ok(paginatedResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }


        #region FUNÇÕES DE APOIO - FABIO CERRI - FARTEC SISTEMAS

        #region GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                throw new Exception("Parametros necessarios nao informados");

            List<Credenciadora> listaCredenciadora = await _repCredenciadora.SelectContext()
                                                            .AsNoTracking()
                                                            .ToListAsync();
            return Ok(listaCredenciadora);
        }
        #endregion GET ALL

        #region GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            if (!ModelState.IsValid)
                throw new Exception("Parametros necessarios nao informados");

            Credenciadora Credenciadora = await _repCredenciadora.SelectContext()                                                            
                                                            .Where(e => e.Id.Equals(id))
                                                            .FirstAsync();
            return Ok(Credenciadora);
        }
        #endregion GET ALL        


        #endregion

        #region FUNÇÕES CRUD - FABIO CERRI - FARTEC SISTEMAS
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Credenciadora model, CancellationToken cancellationToken)
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
                await _repCredenciadora.SaveAsync(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Credenciadora model, CancellationToken cancellationToken)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                await _repCredenciadora.UpdateAsync(model);


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
                Credenciadora Credenciadora = await _repCredenciadora.SelectAsync(id) ?? throw new Exception("Erro ao apagar o Credenciadora com o id fornecido.");

                await _repCredenciadora.DeleteAsync(Credenciadora);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Credenciadora excluida com sucesso"
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
