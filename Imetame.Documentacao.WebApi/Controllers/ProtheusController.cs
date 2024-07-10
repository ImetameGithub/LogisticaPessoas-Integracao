using System.Text.Json;
using Imetame.Documentacao.CrossCutting.Services.Protheus;
using Imetame.Documentacao.CrossCutting.Services.Protheus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Imetame.Documentacao.WebApi.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ProtheusController : Controller
    {
        private readonly IProtheusService _protheusService;
        public ProtheusController(IProtheusService protheusService)
        {
            _protheusService = protheusService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDocumento([FromBody] DocumentoProtheus documento)
        {
            try
            {
                string endPoint = $"/updatedocumento";
                string json = JsonSerializer.Serialize(documento);

                var result = await _protheusService.PostAsync(endPoint, json);
                if (result.IsSuccessStatusCode)
                {
                    return Ok(await result.Content.ReadAsStringAsync());
                }

                return StatusCode((int)result.StatusCode, await result.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
