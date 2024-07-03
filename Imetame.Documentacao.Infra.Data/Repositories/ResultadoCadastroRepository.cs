using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Infra.Data.Context;
using Imetame.Documentacao.Infra.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Repositories
{
    public class ResultadoCadastroRepository : EFRepositoryBase<Domain.Entities.ResultadoCadastro, Guid>, IResultadoCadastroRepository
    {
        public ResultadoCadastroRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(long count, IEnumerable<Domain.Models.ResultadoCadastro> entities)> ListaAsync(Guid idProcessamento, int skip, int take, CancellationToken cancellationToken)
        {
            var query = from c in DbSet
                        select new Domain.Models.ResultadoCadastro()
                        {
                            Id = c.Id,
                            IdProcessamento = c.IdProcessamento,
                            Sucesso = c.Sucesso,
                            NumCad = c.NumCad,
                            Nome = c.Nome,
                            LogString = c.LogString,
                            NumCracha = c.NumCracha,
                            Equipe = c.Equipe,
                            FuncaoAtual = c.FuncaoAtual
                        };

            query = query.Where(q => q.IdProcessamento == idProcessamento);

            var count = await query.LongCountAsync(cancellationToken);

            var entities = await query.OrderBy(q => q.Nome)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);


            return (count, entities);
        }
    }
}
