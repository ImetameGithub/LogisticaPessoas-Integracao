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
    public interface ILogProcessamentoRepository : IRepositoryRead<Entities.LogProcessamento, Guid>, IRepositoryWrite<Entities.LogProcessamento>
    {
        Task<(long count, IEnumerable<Domain.Models.LogProcessamento> entities)> ListaAsync(Guid idProcessamento, DateTime? ultimoLog, int skip, int take, CancellationToken cancellationToken);
        Task<LogProcessamento> GetUltimoLog(Guid idProcessamento, CancellationToken cancellationToken);

    }
}
