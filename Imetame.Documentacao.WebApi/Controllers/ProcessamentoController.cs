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
    [Route("api/[controller]")] 
    [ApiController]
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class ProcessamentoController : Controller
    {
        private readonly IProcessamentoService _service;
        private readonly IBaseRepository<Domain.Entities.Processamento> _repProcessamento;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;



        public ProcessamentoController(IProcessamentoService service, IBaseRepository<Domain.Entities.Processamento> repProcessamento, IConfiguration configuration, IMapper mapper)
        {
            _service = service;
            _repProcessamento = repProcessamento;
            _configuration = configuration;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(typeof(Domain.Models.PaginatedItemsViewModel<Domain.Models.ProcessamentoList>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListarPaginado(CancellationToken cancellationToken, [FromQuery] string query = null, int pageIndex = 0, [FromQuery] int pageSize = 50)
        {
            var model = await _service.ListarPaginadoAsync(query, pageIndex, pageSize, cancellationToken);
            return Ok(model);
        }


        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Domain.Models.Processamento), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Domain.Models.Processamento>> ObterPeloId(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await _service.ObterPeloIdAsync(id, cancellationToken));
        }

        //[HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.Created)]
        //public async Task<ActionResult> Criar([FromBody] Domain.Models.Processamento model, CancellationToken cancellationToken)
        //{
        //     //await _service.CriarAsync(model, cancellationToken);
        //    var processamento = _mapper.Map<Domain.Entities.Processamento>(model);

        //    return CreatedAtAction(nameof(ObterPeloId), new { id = processamento.Id }, processamento);
        //}

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Criar([FromBody] Domain.Models.Processamento model, CancellationToken cancellationToken)
        {
            try
            {
                //await _service.CriarAsync(model, cancellationToken);
                var processamento = _mapper.Map<Domain.Entities.Processamento>(model);

                processamento.Status = StatusProcessamento.Iniciado;
                processamento.InicioProcessamento = DateTime.Now;

                await _repProcessamento.SaveAsync(processamento);

                return CreatedAtAction(nameof(ObterPeloId), new { id = processamento.Id }, processamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorHelper.GetException(ex));
            }
        }

        [HttpGet("ativo")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Domain.Models.Processamento), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Domain.Models.Processamento>> GetProcessamentoAtivo([FromQuery] Guid idPedido, CancellationToken cancellationToken)
        {

            return Ok(await _service.GetProcessamentoAtivo(idPedido, cancellationToken));
        }

        [HttpGet("{id:Guid}/logs")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(List<Domain.Models.LogProcessamento>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<Domain.Models.LogProcessamento>>> GetLogs(Guid id,DateTime? ultimoLog, CancellationToken cancellationToken, int pageIndex = 0, [FromQuery] int pageSize = 50)
        {

            return Ok(await _service.GetLogs(id, ultimoLog, pageIndex, pageSize, cancellationToken));
        }

        [HttpGet("{id:Guid}/resultados")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Domain.Models.ResultadoCadastro>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Domain.Models.ResultadoCadastro>>> GetResultados(Guid id, CancellationToken cancellationToken,  int pageIndex = 0, [FromQuery] int pageSize = 50)
        {

            return Ok(await _service.GetResultados(id, pageIndex, pageSize, cancellationToken));
        }


    }
}
