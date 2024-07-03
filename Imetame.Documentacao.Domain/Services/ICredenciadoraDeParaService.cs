using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Services
{
    public interface ICredenciadoraDeParaService
    {
        Task<Models.PaginatedItemsViewModel<Models.CredenciadoraDeParaList>> ListarPaginadoAsync(string query, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<Models.CredenciadoraDePara> ObterPeloIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Models.CredenciadoraDePara> CriarAsync(Models.CredenciadoraDePara model, CancellationToken cancellationToken);
        Task AtualizarAsync(Models.CredenciadoraDePara model, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
