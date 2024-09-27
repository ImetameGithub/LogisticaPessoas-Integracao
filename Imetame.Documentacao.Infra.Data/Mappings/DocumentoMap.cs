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
    internal class DocumentoMap : BaseMap<Documento>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Documento> builder)
        {
            builder.ToTable(nameof(Documento));

            builder.Property(c => c.Descricao).HasColumnType("varchar(50)").HasMaxLength(255).IsRequired();
            builder.Property(c => c.IdDestra).HasColumnType("varchar(50)").HasMaxLength(255).IsRequired();
            builder.Property(c => c.DescricaoDestra).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            //builder.Property(c => c.IdProtheus).HasColumnType("varchar(50)").HasMaxLength(255).IsRequired();
            //builder.Property(c => c.DescricaoProtheus).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
			builder.Property(c => c.Obrigatorio).HasColumnType("bit").HasDefaultValue(false).IsRequired();


			// Definindo o relacionamento de um para muitos corretamente
			//builder.HasMany(d => d.DocumentoXProtheus) // Documento tem muitos DocumentoXProtheus
			//	   .WithOne(dx => dx.Documento)        // DocumentoXProtheus está relacionado a um Documento
			//	   .HasForeignKey(dx => dx.DocumentoId) // Definir a chave estrangeira como IdDocumento
			//	   .OnDelete(DeleteBehavior.Restrict);
		}
    }
}
