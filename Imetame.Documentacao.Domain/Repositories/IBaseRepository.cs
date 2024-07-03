using Imetame.Documentacao.Domain.Core.Models;
using Imetame.Documentacao.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task ExecuteInTransactionAsync(Action action);
        void Insert(TEntity obj);

        void Update(TEntity obj);

        void Delete(int id);

        IList<TEntity> Select();

        TEntity Select(int id);

        IQueryable<TEntity> SelectByCondition(Expression<Func<TEntity, bool>> expression);

        //Task <IList<TEntity>> SelectAsync();

        Task<IList<TEntity>> SelectAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> SelectAsync(Guid id);

        Task SaveAsync(TEntity obj);
        Task InsertAsync(TEntity obj);

        IQueryable<TEntity> SelectContext();

        Task UpdateAsync(TEntity obj);
        Task DeleteAsync(TEntity obj);
        Task InsertRangeAsync(IList<TEntity> obj);
        Task UpdateRangeAsync(IList<TEntity> obj);
        Task DeleteRangeAsync(IList<TEntity> obj);

        void Dispose();
        Task DisposeAsync();
    }
}
