using Imetame.Documentacao.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Mappings
{
    internal class ColaboradorxPedidoMap : BaseMap<ColaboradorxPedido>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ColaboradorxPedido> builder)
        {
            builder.ToTable(nameof(ColaboradorxPedido));

            builder.Property(c => c.CXP_NUMEROOS)
                .HasColumnType("varchar(20)")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(c => c.CXP_DTINCLUSAO)
                .HasDefaultValue(DateTime.Now)
                .IsRequired();

            builder.HasOne(x => x.Pedido)
                 .WithMany(m => m.ColaboradorxPedido)
                 .HasForeignKey(x => x.CXP_IDPEDIDO)
                 .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Colaborador)
                 .WithMany(m => m.ColaboradorxPedido)
                 .HasForeignKey(x => x.CXP_IDCOLABORADOR)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
