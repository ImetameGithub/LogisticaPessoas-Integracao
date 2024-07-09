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
                        var jsonResponse = await result.Content.ReadAsStringAsync();
                        List<AtividadeEspecifica> listAtividades = new List<AtividadeEspecifica>();

                        ListaAtividadesModel listaModel = JsonSerializer.Deserialize<ListaAtividadesModel>(jsonResponse);

                        foreach (var item in listaModel.LISTA)
                        {
                            AtividadeEspecifica at = new AtividadeEspecifica
                            {
                                Codigo = item.codigo,
                                Descricao = item.descricao,
                                IdDestra = item.id,
                            };

                            listAtividades.Add(at);
                        }

                        // ESTÁ DESSA MANEIRA POIS A CLASSE ENTITY CRIADA NO PROJETO INSERE UM GUID E O BASE REPOSITORIO NÃO SALVA DESSA MANEIRA - TENTEI PDRONIZAR MAS MUDA MUITA COISA - MATHEUS FARTEC
                        listAtividades.ForEach(m => m.Id = new Guid());
                        await _repAtividadeEspecifica.InsertRangeAsync(listAtividades);

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
