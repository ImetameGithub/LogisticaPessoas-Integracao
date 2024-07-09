using Imetame.Documentacao.CrossCutting.Services.Destra;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.WebAPI.Helpers;
using Imetame.Documentacao.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;
using System.Net;
using System.Text;
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
        public DestraController(IConfiguration configuration, IDestraService destraService)
        {
            _configuration = configuration;
            _destraService = destraService;
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
        public async Task<IActionResult> GetDocumentos()
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
        public async Task<IActionResult> GetAtividades()
        {
            try
            {
                AuthResponse response = await Login();

                string endPoint = $"/ativesp/lista";

                if (!response.Erro)
                {
                    var result = await _destraService.GetAsync(endPoint,response.Token);
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
