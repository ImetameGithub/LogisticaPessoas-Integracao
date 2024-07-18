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
    internal class DocumentoxColaboradorMap : BaseMap<DocumentoxColaborador>
    {
        public override void ConfigureEntity(EntityTypeBuilder<DocumentoxColaborador> builder)
        {
            builder.ToTable(nameof(DocumentoxColaborador));

            builder.Property(c => c.DXC_CODPROTHEUS).IsRequired();
            builder.Property(c => c.DXC_CODDESTRA).IsRequired();
            builder.Property(c => c.DXC_BASE64).IsRequired();

            builder.HasOne(x => x.Colaborador)
                 .WithMany(x => x.DocumentosxColaborador)
                 .HasForeignKey(x => x.DXC_IDCOLABORADOR)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
