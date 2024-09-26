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
	internal class DocumentoProtheusMap : BaseMap<DocumentoProtheus>
	{
		public override void ConfigureEntity(EntityTypeBuilder<DocumentoProtheus> builder)
		{
			builder.ToTable(nameof(DocumentoProtheus));

			builder.Property(c => c.IdProtheus)
						.HasColumnType("varchar(255)")
						.HasMaxLength(255)
						.IsRequired();

			builder.Property(c => c.DescricaoProtheus)
						.HasColumnType("varchar(255)")
						.HasMaxLength(255)
						.IsRequired();

			builder.HasOne(x => x.Documento)
						.WithMany()
						.HasForeignKey(x => x.IdDocumento)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
