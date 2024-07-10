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
    internal class ColaboradorMap : BaseMap<Colaborador>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Colaborador> builder)
        {
            builder.ToTable(nameof(Colaborador));

            builder.Property(c => c.Matricula)
             .HasMaxLength(8)
             .IsRequired();

            builder.Property(c => c.Cracha)
             .HasMaxLength(8)
             .IsRequired();

            builder.Property(c => c.Nome)
             .IsRequired();

        }
    }
}
