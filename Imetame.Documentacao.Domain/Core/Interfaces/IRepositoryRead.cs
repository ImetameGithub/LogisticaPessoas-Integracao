using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Core.Interfaces
{
    public interface IRepositoryRead<TEntity, TKey> : IDisposable where TEntity : class
    {

        ValueTask<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken);
        IQueryable<TEntity> Todos();


    }
}
