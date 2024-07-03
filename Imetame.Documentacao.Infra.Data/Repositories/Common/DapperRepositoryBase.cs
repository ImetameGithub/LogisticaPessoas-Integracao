using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Infra.Data.Mappings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Repositories.Common
{
    public abstract class DapperRepositoryBase<TEntity, TKey> : IRepositoryWrite<TEntity>, IRepositoryRead<TEntity, TKey> where TEntity : class
    {
        private readonly IConfiguration _configuration;
        protected readonly SqlConnection conn;

        public DapperRepositoryBase(IConfiguration configuration)
        {
            _configuration = configuration;
            if (FluentMapper.EntityMaps.IsEmpty)
            {
                FluentMapper.Initialize(c =>
                {
                    //c.AddMap(new HistoricoDapperMap());

                    c.ForDommel();
                });
            }
            conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        private bool _disposed = false;

        public void Add(TEntity obj) => conn.Insert(obj);

        public void Update(TEntity obj) => conn.Update(obj);




        public void Remove(TEntity obj) => conn.Delete(obj);


        public int SaveChanges()
        {
            return 0;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return 0;
        }

        ~DapperRepositoryBase() =>
           Dispose();

        public void Dispose()
        {
            if (!_disposed)
            {
                conn.Close();
                conn.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }




        public IQueryable<TEntity> Todos()
        {
            throw new NotImplementedException();
        }

        public ValueTask<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken) => new ValueTask<TEntity>(conn.GetAsync<TEntity>(id));

    }
}
