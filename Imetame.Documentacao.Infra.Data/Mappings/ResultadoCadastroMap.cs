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
    internal class ResultadoCadastroMap : BaseMap<ResultadoCadastro>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ResultadoCadastro> builder)
        {
            builder.ToTable(nameof(ResultadoCadastro));

            builder.Property(c => c.IdProcessamento).IsRequired();
            builder.Property(c => c.Sucesso).IsRequired();
            builder.Property(c => c.NumCad).HasColumnType("varchar(50)").HasMaxLength(50).IsRequired();
            builder.Property(c => c.Nome).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            //builder.Property(c => c.LogString).HasColumnType("text").IsRequired();
            builder.Property(c => c.LogString).IsRequired().HasColumnType("nvarchar(max)"); ;
            builder.Property(c => c.NumCracha).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired(false).HasDefaultValue(null);

            //builder.Property(c => c.Equipe).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            builder.Property(c => c.Equipe).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired(false).HasDefaultValue(null);
            //builder.Property(c => c.FuncaoAtual).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();
            builder.Property(c => c.FuncaoAtual).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired(false).HasDefaultValue(null);


            builder.HasOne(x => x.Processamento)
                    .WithMany()
                    .HasForeignKey(p => p.IdProcessamento);
        }
    }
}
