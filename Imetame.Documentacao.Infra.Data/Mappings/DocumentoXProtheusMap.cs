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
	internal class DocumentoXProtheusMap : BaseMap<DocumentoXProtheus>
	{
		public override void ConfigureEntity(EntityTypeBuilder<DocumentoXProtheus> builder)
		{
			builder.ToTable(nameof(DocumentoXProtheus));

			builder.Property(c => c.IdProtheus)
						.HasColumnType("varchar(255)")
						.HasMaxLength(255)
						.IsRequired();

			builder.Property(c => c.DescricaoProtheus)
						.HasColumnType("varchar(255)")
						.HasMaxLength(255)
						.IsRequired();

			builder.HasOne(x => x.Documento)
						.WithMany(x=>x.DocumentoXProtheus)
						.HasForeignKey(x => x.DocumentoId)
						.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
