using Imetame.Documentacao.Domain.Core.Enum;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Imetame.Documentacao.Domain.Entities;
using Microsoft.Extensions.Configuration;
using DocumentFormat.OpenXml.Spreadsheet;
using AutoMapper;
using Imetame.Documentacao.WebAPI.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using OpenIddict.Validation.AspNetCore;

namespace Imetame.Documentacao.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ProcessamentoFartecController : Controller
    {
        private readonly IProcessamentoService _service;
        private readonly IBaseRepository<Domain.Entities.Processamento> _repProcessamento;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;



        public ProcessamentoFartecController(IProcessamentoService service, IBaseRepository<Domain.Entities.Processamento> repProcessamento, IConfiguration configuration, IMapper mapper)
        {
            _service = service;
            _repProcessamento = repProcessamento;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Domain.Models.Processamento model, CancellationToken cancellationToken)
        {
            try
            {
                //await _service.CriarAsync(model, cancellationToken);
                var processamento = _mapper.Map<Domain.Entities.Processamento>(model);

                processamento.Status = StatusProcessamento.Iniciado;
                processamento.InicioProcessamento = DateTime.Now;

                await _repProcessamento.SaveAsync(processamento);

                return Ok(processamento.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Domain.Models.Processamento), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Domain.Models.Processamento>> ObterPeloId(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _service.ObterPeloIdAsync(id, cancellationToken));
        }

    }
}
