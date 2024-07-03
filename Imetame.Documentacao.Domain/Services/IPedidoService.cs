using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public interface IPedidoService
    {
        Task<Models.PaginatedItemsViewModel<Models.PedidoList>> ListarPaginadoAsync(string query, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<Models.Pedido> ObterPeloIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Models.Pedido> CriarAsync(Models.Pedido model, CancellationToken cancellationToken);
        Task AtualizarAsync(Models.Pedido model, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
