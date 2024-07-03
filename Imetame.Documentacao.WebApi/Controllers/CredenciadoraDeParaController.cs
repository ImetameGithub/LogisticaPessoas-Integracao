using Imetame.Documentacao.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System.Net;

namespace Imetame.Documentacao.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class CredenciadoraDeParaController : ControllerBase
    {
        private readonly ICredenciadoraDeParaService _service;



        public CredenciadoraDeParaController(ICredenciadoraDeParaService service)
        {
            _service = service;

        }


        [HttpGet]
        [ProducesResponseType(typeof(Domain.Models.PaginatedItemsViewModel<Domain.Models.CredenciadoraDeParaList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListarPaginado(CancellationToken cancellationToken, [FromQuery] string query = null, int pageIndex = 0, [FromQuery] int pageSize = 50)
        {
            var model = await _service.ListarPaginadoAsync(query, pageIndex, pageSize, cancellationToken);
            return Ok(model);
        }


        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Domain.Models.CredenciadoraDePara), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Domain.Models.CredenciadoraDePara>> ObterPeloId(Guid id, CancellationToken cancellationToken)
        {

            return Ok(await _service.ObterPeloIdAsync(id, cancellationToken));
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> Criar([FromBody] Domain.Models.CredenciadoraDePara model, CancellationToken cancellationToken)
        {
            var created = await _service.CriarAsync(model, cancellationToken);
            return CreatedAtAction(nameof(ObterPeloId), new { id = created.Id }, created);
        }



        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> AtualizarAsync([FromBody] Domain.Models.CredenciadoraDePara model, CancellationToken cancellationToken)
        {
            await _service.AtualizarAsync(model, cancellationToken);
            return CreatedAtAction(nameof(ObterPeloId), new { id = model.Id }, model);
        }


        [HttpDelete("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }


    }
}
