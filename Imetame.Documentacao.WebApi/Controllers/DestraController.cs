using Imetame.Documentacao.CrossCutting.Services.Destra;
using Imetame.Documentacao.CrossCutting.Services.Destra.Models;
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
    public class DestraController : Controller
    {
        // private readonly IBaseRepository<Pedido> _repPedido;
        private readonly IConfiguration _configuration;
        public DestraController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            try
            {
                var authRequest = new AuthDestra
                {
                    Login = $"{_configuration["UsrApiDestra"]}",
                    Pwd = $"{_configuration["PwdApiDestra"]}"
                };

                DestraService destraService = new DestraService();

                AuthResponse response = await destraService.AuthenticateAsync(authRequest);

                if (response.Erro)
                {
                    if (response.MensagemErro == "Credenciais invalidas.")
                    {
                        return Unauthorized(new { message = response.MensagemErro });
                    }
                    else
                    {
                        return StatusCode(500, new { message = response.MensagemErro });
                    }
                }

                return Ok(response.Token);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}
