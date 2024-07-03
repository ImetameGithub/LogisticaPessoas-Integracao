using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Domain.Entities;

using Imetame.Documentacao.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Imetame.Documentacao.Infra.Data.Repositories.Common;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using Imetame.Documentacao.Domain.Models;

namespace Imetame.Documentacao.Infra.Data.Repositories
{
    public class CredenciadoraDeParaRepository : EFRepositoryBase<Domain.Entities.CredenciadoraDePara, Guid>, ICredenciadoraDeParaRepository
    {
        public CredenciadoraDeParaRepository(ApplicationDbContext context) : base(context)
        {
        }



       
        public Task<bool> JaExisteAsync(Guid id, string credenciadora, string de, string para, CancellationToken cancellationToken)
        {
            return DbSet.AnyAsync(q => q.Id != id && q.Credenciadora == credenciadora && q.De == de && q.Para == para);
        }

        public async Task<(long count, IEnumerable<Domain.Models.CredenciadoraDeParaList> entities)> ListaAsync(string texto, int skip, int take, CancellationToken cancellationToken)
        {

            var query = from c in DbSet
                        select new CredenciadoraDeParaList()
                        {
                            Id = c.Id,
                            Credenciadora = c.Credenciadora,
                            De = c.De,
                            Para = c.Para
                        };
                

            if (!string.IsNullOrEmpty(texto))
            {
                query = query.Where(q => q.Credenciadora.Contains(texto) || q.De.Contains(texto) || q.Para.Contains(texto));
            }

           
            var count = await query.LongCountAsync(cancellationToken);

            var entities = await query.OrderBy(q => q.Credenciadora).ThenBy(q=>q.De).ThenBy(q => q.Para)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);


            return (count, entities);
        }

        public Task<List<Domain.Entities.CredenciadoraDePara>> ListarPorCredenciadoraAsync(string credenciadora)
        {
                return DbSet.Where(q => q.Credenciadora.ToUpper() == credenciadora.ToUpper()).ToListAsync();
        }
    }
}
