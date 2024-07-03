using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Validation;
using Imetame.Documentacao.Domain.Core.Enum;
using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Domain.Repositories;
using Imetame.Documentacao.Infra.Data.Context;
using Imetame.Documentacao.Infra.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Repositories
{
    public class ProcessamentoRepository : EFRepositoryBase<Domain.Entities.Processamento, Guid>, IProcessamentoRepository
    {
        private readonly IPedidoRepository _pedidoRepository;

        public ProcessamentoRepository(ApplicationDbContext context, IPedidoRepository pedidoRepository) : base(context)
        {
            this._pedidoRepository = pedidoRepository;
        }

        public Task<List<Domain.Entities.LogProcessamento>> GetLogs(Guid id, DateTime? ultimoLog)
        {
            throw new NotImplementedException();
        }

        public override async ValueTask<Domain.Entities.Processamento> GetByIdAsync(Guid id, CancellationToken cancellationToken) {
            var processamento = await DbSet.FirstOrDefaultAsync(p => p.Id == id);
            processamento.Pedido = await _pedidoRepository.GetByIdAsync(processamento.IdPedido, cancellationToken);
            return processamento;
        }

        public async Task<Domain.Entities.Processamento> GetProcessamentoAtivo(Guid idPedido)
        {
            return await DbSet.FirstOrDefaultAsync(q => q.IdPedido == idPedido && q.Status == StatusProcessamento.Executando);
        }
              

        public async Task<(long count, IEnumerable<ProcessamentoList> entities)> ListaAsync(string texto, int skip, int take, CancellationToken cancellationToken)
        {
            var query = from c in DbSet
                        select new ProcessamentoList()
                        {
                            Id = c.Id,
                            IdPedido = c.IdPedido,
                            Status = c.Status,
                            InicioProcessamento = c.InicioProcessamento,
                            FinalProcessamento = c.FinalProcessamento,
                            Pedido = c.Pedido.NumPedido,
                            OssString = c.OssString
                        };


            if (!string.IsNullOrEmpty(texto))
            {
                query = query.Where(q => q.Pedido.Contains(texto) || q.OssString.Contains(texto) );
            }


            var count = await query.LongCountAsync(cancellationToken);

            var entities = await query.OrderByDescending(q => q.InicioProcessamento).ThenBy(q => q.Pedido)
                .Skip(skip)
                .Take(take)
                .ToListAsync(cancellationToken);


            return (count, entities);
        }
    }
}
