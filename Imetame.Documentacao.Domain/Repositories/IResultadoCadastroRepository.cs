using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Repositories
{
    public interface IResultadoCadastroRepository : IRepositoryRead<Entities.ResultadoCadastro, Guid>, IRepositoryWrite<Entities.ResultadoCadastro>
    {
        Task<(long count, IEnumerable<Models.ResultadoCadastro> entities)> ListaAsync(Guid idProcessamento, int skip, int take, CancellationToken cancellationToken);

    }
}
