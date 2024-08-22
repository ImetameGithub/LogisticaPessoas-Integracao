using Imetame.Documentacao.CrossCutting.Services.Destra;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;
using Imetame.Documentacao.Domain.Dto;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Infra.Data.Migrations;
using Irony.Parsing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
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
                        var content = await result.Content.ReadAsStringAsync();
                        return Ok(content);
                        //return Ok(await result.Content.ReadAsStringAsync());
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
        public async Task<IActionResult> AddPedidoxDireta([FromBody] IncluirPedidoxDireta pedidoxDireta)
        {
            try
            {
                AuthResponse response = await Login();

                if (!response.Erro)
                {
                    string endPoint = $"/pedido/incluir";
                    string json = JsonSerializer.Serialize(pedidoxDireta);

                    var result = await _destraService.PostAsync(endPoint, json, response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        return Ok(content);
                        //return Ok(await result.Content.ReadAsStringAsync());
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
        public async Task<IActionResult> AddColaboradorPedido([FromBody] Equipe equipe)
        {
            try
            {
                AuthResponse response = await Login();

                if (!response.Erro)
                {
                    string endPoint = $"/pedido/func/incluir";
                    string json = JsonSerializer.Serialize(equipe);

                    var result = await _destraService.PostAsync(endPoint, json, response.Token);
                    if (result.IsSuccessStatusCode)
                    {
                        var content = await result.Content.ReadAsStringAsync();
                        return Ok(content);
                        //return Ok(await result.Content.ReadAsStringAsync());
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
        
        [HttpGet("{Id}")]
        public async Task<string> GetDocumentosRequeridos(string Id)
        {
            try
            {
                AuthResponse response = await Login();

                string endPoint = $"/service/docto/funcionario/lista?atividadeEspecifica=" + Id;

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
                throw new Exception("Erro ao obter documentos da atividade atividades", ex);
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
        
        [HttpPost]
        public async Task<HttpResponseMessage> EnviarDocumentoParaApiDoCliente(DocumentoDestra documento, string NomeDoc)
        {
            using (var client = new HttpClient())
            {
                string endPoint = $"https://api.destra.armata.cloud/homolog/api/v1/service/docto/funcionario?=";
                var form = new MultipartFormDataContent();

                AuthResponse response = await Login();

                form.Add(new StringContent(documento.cpf), "cpf");
                form.Add(new StringContent(documento.idDocto.ToString()), "idDocto");
                form.Add(new StringContent(documento.validade), "validade");

                var arquivoContent = new ByteArrayContent(documento.arquivo);
                arquivoContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                form.Add(arquivoContent, "arquivo", NomeDoc);

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",response.Token);

                var result = await client.PostAsync(endPoint, form);

                return result;
            }
        }

        [HttpPost]
        public async Task<IActionResult> IncluirColaboradorPedido([FromBody] IncluirColaboradorPedido documento)
        {
            try
            {
                AuthResponse response = await Login();

                if (!response.Erro)
                {
                    string endPoint = $"/pedido/atualizar";
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

        [HttpPost]
        public async Task<IActionResult> IncluirDiretaPedido([FromBody] IncluirPedidoxDireta documento)
        {
            try
            {
                AuthResponse response = await Login();

                if (!response.Erro)
                {
                    string endPoint = $"/pedido/incluir";
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
