
using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Exceptions;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CadastrosController : ControllerBase
    {
        private readonly ICadastroService _service;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);


        public CadastrosController(ICadastroService service)
        {
            _service = service;

        }





        // [HttpPost()]
        // public async Task<IActionResult> Execucao([FromBody] CadastroModel model, CancellationToken cancellationToken)
        // {

            
        //     await semaphoreSlim.WaitAsync();
        //     try// favor deixar o try catch para que o semaphoreSlim seja liberado
        //     {
        //         var resp = await _service.CadastrarAsync(model, cancellationToken);
        //         return Ok(resp);
        //     //}
        //     //catch (Exception e)
        //     //{
        //     //    resp = new CadastroResponse() { Log = e.Message };
                
        //     }
        //     finally
        //     {
        //         semaphoreSlim.Release();
        //     }

            



        // }



    }
}
