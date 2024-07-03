using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Infra.Data.Context;
using Imetame.Documentacao.Infra.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Repositories
{
    public class LogProcessamentoRepository : EFRepositoryBase<Domain.Entities.LogProcessamento, Guid>, ILogProcessamentoRepository
    {
        public LogProcessamentoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Domain.Entities.LogProcessamento> GetUltimoLog(Guid idProcessamento, CancellationToken cancellationToken)
        {
            return await DbSet.Where(q => q.IdProcessamento == idProcessamento).OrderByDescending(q=>q.DataEvento).FirstOrDefaultAsync();
        }

        public async Task<(long count, IEnumerable<LogProcessamento> entities)> ListaAsync(Guid idProcessamento, DateTime? ultimoLog, int skip, int take, CancellationToken cancellationToken)
        {
            var query = from c in DbSet
                        select new LogProcessamento()
                        {
                            Id = c.Id,
                            IdProcessamento = c.IdProcessamento,
                            Evento = c.Evento,
                            DataEvento = c.DataEvento,
                        };

            query = query.Where(q => q.IdProcessamento== idProcessamento);

            if (ultimoLog.HasValue)
            {
                query = query.Where(q => q.DataEvento > ultimoLog.Value);
            }


            var count = await query.LongCountAsync(cancellationToken);

            var entities = await query.OrderBy(q => q.DataEvento)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);


            return (count, entities);
        }
    }
}
