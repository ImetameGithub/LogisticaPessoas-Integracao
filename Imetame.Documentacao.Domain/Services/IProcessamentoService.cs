using Imetame.Documentacao.Domain.Core.Enum;
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
    public interface IProcessamentoService
    {
        Task<Models.Processamento> GetProcessamentoAtivo(Guid idPedido, CancellationToken cancellationToken);
        Task<Models.PaginatedItemsViewModel<Models.ProcessamentoList>> ListarPaginadoAsync(string query, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<Models.Processamento> ObterPeloIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Models.Processamento> CriarAsync(Models.Processamento model, CancellationToken cancellationToken);
        Task AtualizarAsync(Models.Processamento model, CancellationToken cancellationToken);
        Task AtualizarStatusAsync(Guid id, StatusProcessamento statusProcessamento, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<Models.PaginatedItemsViewModel<Models.LogProcessamento>> GetLogs(Guid id,DateTime? ultimoLog, int skip, int take, CancellationToken cancellationToken);

        Task<Models.PaginatedItemsViewModel<Models.ResultadoCadastro>> GetResultados(Guid id, int skip, int take, CancellationToken cancellationToken);

    }
}
