using Imetame.Documentacao.Domain.Core.Enum;
using Imetame.Documentacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Mappings
{
    internal class ProcessamentoMap : BaseMap<Processamento>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Processamento> builder)
        {
            builder.ToTable(nameof(Processamento));


            builder.Property(c => c.IdPedido).IsRequired();
            builder.Property(c => c.OssString).HasColumnType("text").IsRequired();
            builder.Property(c => c.FinalProcessamento);
            builder.Property(c => c.InicioProcessamento).IsRequired();
            builder.Property(c => c.Status).IsRequired();


            builder.HasOne(x => x.Pedido)
            .WithMany()
            .HasForeignKey(p => p.IdPedido);
        }
    }
}
