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
    public class PedidoRepository : EFRepositoryBase<Domain.Entities.Pedido, Guid>, IPedidoRepository
    {
        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<bool> JaExisteAsync(Guid id, string numPedido, CancellationToken cancellationToken)
        {
            return DbSet.AnyAsync(q => q.Id != id && q.NumPedido == numPedido);
        }

        public async Task<(long count, IEnumerable<PedidoList> entities)> ListaAsync(string texto, int skip, int take, CancellationToken cancellationToken)
        {

            var query = from c in DbSet
                        select new PedidoList()
                        {
                            Id = c.Id,
                            Credenciadora = c.IdCredenciadora,
                            NumPedido = c.NumPedido,
                            Unidade = c.Unidade
                        };


            if (!string.IsNullOrEmpty(texto))
            {
                query = query.Where(q => q.Unidade.Contains(texto) || q.NumPedido.Contains(texto));
            }


            var count = await query.LongCountAsync(cancellationToken);

            var entities = await query.OrderBy(q => q.NumPedido).ThenBy(q => q.Unidade).ThenBy(q => q.Credenciadora)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);


            return (count, entities);
        }
    }
}
