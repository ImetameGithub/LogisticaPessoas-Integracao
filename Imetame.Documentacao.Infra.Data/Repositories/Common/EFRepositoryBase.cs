using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Repositories.Common
{
    public abstract class EFRepositoryBase<TEntity, TKey> : IRepositoryWrite<TEntity>, IRepositoryRead<TEntity, TKey> where TEntity : class
    {
        protected readonly ApplicationDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public EFRepositoryBase(ApplicationDbContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }



        public virtual void Add(TEntity obj)
        {
            DbSet.Add(obj);
        }


        public virtual void Update(TEntity obj)
        {
            DbSet.Update(obj);
        }



        public void Remove(TEntity obj)
        {
            DbSet.Remove(obj);
        }

        public int SaveChanges()
        {
            return Db.SaveChanges();
        }



        public void Dispose()
        {
            Db.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Db.SaveChangesAsync(cancellationToken);
        }

        protected string StringParaLike(string s)
        {
            return "%" + s.Replace("[", "[[]").Replace("%", "[%]") + "%";
        }

        public virtual ValueTask<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken)
        {
            return DbSet.FindAsync(id);

        }


        public IQueryable<TEntity> Todos()
        {
            return DbSet;
        }

        protected string Where(Dictionary<string, string> filters)
        {
            if (filters == null || filters.Count == 0) return "";
            bool first = true;
            string where = "Where ";
            foreach (var key in filters.Keys)
            {
                if (first)
                    first = false;
                else where += " and ";
                where += $"key = @{key}";
            }
            return where;

        }
    }
}
