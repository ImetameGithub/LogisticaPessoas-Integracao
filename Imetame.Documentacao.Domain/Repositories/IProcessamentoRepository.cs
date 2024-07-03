using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Repositories
{
    public interface IProcessamentoRepository : IRepositoryRead<Entities.Processamento, Guid>, IRepositoryWrite<Entities.Processamento>
    {
        Task<(long count, IEnumerable<Domain.Models.ProcessamentoList> entities)> ListaAsync(string query, int skip, int take, CancellationToken cancellationToken);
        Task<Entities.Processamento> GetProcessamentoAtivo(Guid idPedido);
       
    }
}
