//using Imetame.Documentacao.Domain.Entities;
//using Imetame.Documentacao.Domain.Repositories;
//using Imetame.Documentacao.Infra.Data.Context;
//using Imetame.Documentacao.Infra.Data.Repositories.Common;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;


//namespace Imetame.Documentacao.Infra.Data.Repositories
//{
//    public class TarefaEfRepository : EFRepositoryBase<Domain.Entities.Tarefa, Guid>, ITarefaRepository
//    {

//        public TarefaEfRepository(ApplicationDbContext context) : base(context)
//        {

//        }

//        public Task<bool> JaExisteAsync(Guid id, string nome, CancellationToken cancellationToken)
//        {
//            return DbSet.AnyAsync(q => q.Id != id && q.Nome == nome);
//        }

//        public async Task<(long count, IEnumerable<Domain.Entities.Tarefa> entities)> ListaAsync(string texto, Domain.Entities.Tarefa.StatusEnum? status, int skip, int take, CancellationToken cancellationToken)
//        {
//            var query = DbSet.AsQueryable();
//            if (!string.IsNullOrEmpty(texto))
//            {
//                query = query.Where(q => q.Nome.Contains(texto) || q.Descricao.Contains(texto));
//            }

//            if (status != null)
//            {
//                query = query.Where(q => q.Status == status.Value);
//            }
//            var count = await query.LongCountAsync(cancellationToken);

//            var entities = await query.OrderBy(q => q.Nome)
//                .Skip(skip)
//                .Take(take)
//                .ToListAsync(cancellationToken);

//            return (count, entities);

//        }

//        public async Task<IEnumerable<Tarefa>> ListaAsync(string texto, Tarefa.StatusEnum? status, CancellationToken cancellationToken)
//        {
//            var query = DbSet.AsQueryable();
//            if (!string.IsNullOrEmpty(texto))
//            {
//                query = query.Where(q => q.Nome.Contains(texto) || q.Descricao.Contains(texto));
//            }

//            if (status != null)
//            {
//                query = query.Where(q => q.Status == status.Value);
//            }

//            return await query.OrderBy(q => q.Nome).ToListAsync();
//        }
//    }
//}
