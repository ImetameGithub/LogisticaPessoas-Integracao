using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Imetame.Documentacao.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AtividadeEspecificaController : Controller
    {
        private readonly IBaseRepository<AtividadeEspecifica> _repAtividadeEspecifica;
        private readonly IConfiguration _configuration;
        public AtividadeEspecificaController(IBaseRepository<AtividadeEspecifica> repAtividadeEspecifica, IConfiguration configuration)
        {
            _repAtividadeEspecifica = repAtividadeEspecifica;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaginated(int page = 1, int pageSize = 10, string filtro = "")
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Parametros necessarios nao informados");

                int offset = (page - 1) * pageSize;

                IQueryable<AtividadeEspecifica> query = _repAtividadeEspecifica.SelectContext()
                                                             .OrderBy(x => x.Codigo);
                if (!string.IsNullOrEmpty(filtro))
                    query = query.Where(q => q.Codigo.Contains(filtro) || q.Descricao.Contains(filtro));

                int totalCount = await query.CountAsync();

                IList<AtividadeEspecifica> listAtividadeEspecificas = query.Skip(offset)
                                                             .Take(pageSize).ToList();

                PaginatedResponse<AtividadeEspecifica> paginatedResponse = new PaginatedResponse<AtividadeEspecifica>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = listAtividadeEspecificas
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

            List<AtividadeEspecifica> listaAtividadeEspecifica = await _repAtividadeEspecifica.SelectContext()
                                                            .ToListAsync();
            return Ok(listaAtividadeEspecifica);
        }
        #endregion GET ALL

        #region GET
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            if (!ModelState.IsValid)
                throw new Exception("Parametros necessarios nao informados");

            AtividadeEspecifica AtividadeEspecifica = await _repAtividadeEspecifica.SelectContext()
                                                            .Where(e => e.Id.Equals(id))
                                                            .FirstAsync();
            return Ok(AtividadeEspecifica);
        }
        #endregion GET ALL        


        #endregion

        #region FUNÇÕES CRUD - MATHEUS MONFREIDES FARTEC SISTEMAS
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AtividadeEspecifica model, CancellationToken cancellationToken)
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
                await _repAtividadeEspecifica.SaveAsync(model);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AtividadeEspecifica model, CancellationToken cancellationToken)
        {
            try
            {
                StringBuilder erro = new StringBuilder();
                if (!ModelState.IsValid)
                {
                    erro = ErrorHelper.GetErroModelState(ModelState.Values);
                    throw new Exception("Falha ao Salvar Dados.\n" + erro);
                }

                await _repAtividadeEspecifica.UpdateAsync(model);


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
                AtividadeEspecifica AtividadeEspecifica = await _repAtividadeEspecifica.SelectAsync(id) ?? throw new Exception("Erro ao apagar o AtividadeEspecifica com o id fornecido.");

                await _repAtividadeEspecifica.DeleteAsync(AtividadeEspecifica);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Atividade Especifica excluida com sucesso"
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

