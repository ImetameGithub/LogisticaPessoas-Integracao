using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public interface ICadastroService
    {
        Task<List<Entities.ResultadoCadastro>> CadastrarAsync(CadastroModel model, CancellationToken CancellationToken);
    }
}
