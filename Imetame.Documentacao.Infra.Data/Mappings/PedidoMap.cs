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
    internal class PedidoMap : BaseMap<Pedido>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable(nameof(Pedido));

            builder.Property(c => c.NumPedido).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            builder.Property(c => c.Unidade).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            //builder.Property(c => c.Credenciadora).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();

            builder.HasOne(x => x.Credenciadora)
               .WithMany(m => m.Pedidos)
               .HasForeignKey(x => x.IdCredenciadora)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
