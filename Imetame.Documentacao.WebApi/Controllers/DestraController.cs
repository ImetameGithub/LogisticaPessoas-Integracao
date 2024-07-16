using Imetame.Documentacao.CrossCutting.Services.Destra;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Imetame.Documentacao.WebApi.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    public class DestraController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IDestraService _destraService;
        private readonly IBaseRepository<Domain.Entities.AtividadeEspecifica> _repAtividadeEspecifica;

        public DestraController(IConfiguration configuration, IDestraService destraService, IBaseRepository<Domain.Entities.AtividadeEspecifica> repAtividadeEspecifica)
        {
            _configuration = configuration;
            _destraService = destraService;
            _repAtividadeEspecifica = repAtividadeEspecifica;
        }

        private async Task<AuthResponse> Login()
        {
            try
            {
                var authRequest = new AuthDestra
                {
                    Login = $"{_configuration["UsrApiDestra"]}",
                    Pwd = $"{_configuration["PwdApiDestra"]}"
                };

                AuthResponse response = await _destraService.AuthAsync(authRequest);
                
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

             
        [HttpGet("{cpf}")]
        public async Task<IActionResult> GetColaborador(string cpf)
        {
            try
            {
                AuthResponse response = await Login();

                string endPoint = $"/service/funcionario?cpf={cpf}";

                if (!response.Erro)
                {
                    var result = await _destraService.GetAsync(endPoint, response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        return Ok(await result.Content.ReadAsStringAsync());
                    }

                    return StatusCode((int)result.StatusCode, await result.Content.ReadAsStringAsync());                    
                }
                else
                {
                    return BadRequest(response.MensagemErro);
                }
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddColaborador([FromBody] ColaboradorDestra colaborador)
        {
            try
            {
                AuthResponse response = await Login();

                if (!response.Erro)
                {
                    string endPoint = $"/service/funcionario";
                    string json = JsonSerializer.Serialize(colaborador);

                    var result = await _destraService.PostAsync(endPoint, json, response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        return Ok(await result.Content.ReadAsStringAsync());
                    }

                    return StatusCode((int)result.StatusCode, await result.Content.ReadAsStringAsync());
                }
                else
                {
                    return BadRequest(response.MensagemErro);
                }                
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet]
        public async Task<string> GetDocumentos()
        {
            try
            {
                AuthResponse response = await Login();

                string endPoint = $"/service/docto/funcionario/lista?=";

                if (!response.Erro)
                {
                    var result = await _destraService.GetAsync(endPoint,response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStringAsync();
                    }

                    throw new Exception(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception(response.MensagemErro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter documentos", ex);
            }
        }
        
        [HttpGet]
        public async Task<string> GetAtividades()
        {
            try
            {
                AuthResponse response = await Login();

                string endPoint = $"/ativesp/lista";

                if (!response.Erro)
                {
                    var result = await _destraService.GetAsync(endPoint, response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        return await result.Content.ReadAsStringAsync();
                    }

                    throw new Exception(await result.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception(response.MensagemErro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter atividades", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDocumento([FromBody] DocumentoDestra documento)
        {
            try
            {
                AuthResponse response = await Login();

                if (!response.Erro)
                {
                    string endPoint = $"/service/docto/funcionario?=";
                    string json = JsonSerializer.Serialize(documento);

                    var result = await _destraService.PostAsync(endPoint, json, response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        return Ok(await result.Content.ReadAsStringAsync());
                    }

                    return StatusCode((int)result.StatusCode, await result.Content.ReadAsStringAsync());
                }
                else
                {
                    return BadRequest(response.MensagemErro);
                }                
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
