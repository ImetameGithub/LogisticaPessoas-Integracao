using Imetame.Documentacao.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Repositories
{
    public interface ICredenciadoraDeParaRepository : IRepositoryRead<Entities.CredenciadoraDePara, Guid>, IRepositoryWrite<Entities.CredenciadoraDePara>
    {

        

        Task<bool> JaExisteAsync(Guid id, string credenciadora, string de, string para, CancellationToken cancellationToken);
        Task<(long count, IEnumerable<Domain.Models.CredenciadoraDeParaList> entities)> ListaAsync(string query, int skip, int take, CancellationToken cancellationToken);
        Task<List<Entities.CredenciadoraDePara>> ListarPorCredenciadoraAsync(string credenciadora);
    }
}
