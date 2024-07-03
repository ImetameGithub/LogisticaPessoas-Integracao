using Imetame.Documentacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


    namespace Imetame.Documentacao.Infra.Data.Mappings
{
    internal class CredenciadoraDeParaMap : BaseMap<CredenciadoraDePara>
    {
        public override void ConfigureEntity(EntityTypeBuilder<CredenciadoraDePara> builder)
        {
            builder.ToTable(nameof(CredenciadoraDePara));

            builder.Property(c => c.Credenciadora)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(c => c.De)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(c => c.Para)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255)
                .IsRequired();



        }

    }

}
