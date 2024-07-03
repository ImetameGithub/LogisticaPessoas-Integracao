using Imetame.Documentacao.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Repositories
{
    public interface IPedidoRepository : IRepositoryRead<Entities.Pedido, Guid>, IRepositoryWrite<Entities.Pedido>
    {
        Task<bool> JaExisteAsync(Guid id, string numPedido, CancellationToken cancellationToken);
        Task<(long count, IEnumerable<Domain.Models.PedidoList> entities)> ListaAsync(string query, int skip, int take, CancellationToken cancellationToken);
    }
}
