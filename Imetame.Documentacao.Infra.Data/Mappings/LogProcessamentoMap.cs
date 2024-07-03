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
    internal class LogProcessamentoMap : BaseMap<LogProcessamento>
    {
        public override void ConfigureEntity(EntityTypeBuilder<LogProcessamento> builder)
        {
            builder.ToTable(nameof(LogProcessamento));


            builder.Property(c => c.IdProcessamento).IsRequired();
            builder.Property(c => c.DataEvento).IsRequired();
            builder.Property(c => c.Evento).HasColumnType("varchar(500)").HasMaxLength(500).IsRequired();

        builder.HasOne(x => x.Processamento)
            .WithMany()
            .HasForeignKey(p => p.IdProcessamento);
        }
    }
}
