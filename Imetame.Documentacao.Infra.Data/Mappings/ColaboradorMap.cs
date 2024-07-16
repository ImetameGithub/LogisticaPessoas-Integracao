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
             .IsRequired();

            builder.Property(c => c.Nome)
             .IsRequired();

            builder.Property(c => c.Codigo_Funcao)
             .IsRequired();

            builder.Property(c => c.Nome_Funcao)
             .IsRequired();

            builder.Property(c => c.Codigo_Equipe)
             .IsRequired();

            builder.Property(c => c.Nome_Equipe)
             .IsRequired();

            builder.Property(c => c.SincronizadoDestra)
             .HasDefaultValue(false)
             .IsRequired();

            builder.Property(c => c.Perfil)
             .IsRequired();

            builder.Property(c => c.Codigo_OS)
             .IsRequired();

            builder.Property(c => c.Nome_OS)
             .IsRequired();

        }
    }
}
