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
    internal class ColaboradorxAtividadeMap : BaseMap<ColaboradorxAtividade>
    {
        public override void ConfigureEntity(EntityTypeBuilder<ColaboradorxAtividade> builder)
        {
            builder.ToTable(nameof(ColaboradorxAtividade));

            builder.HasOne(x => x.AtividadeEspecifica)
                 .WithMany()
                 .HasForeignKey(x => x.CXA_IDATIVIDADE_ESPECIFICA)
                 .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Colaborador)
                 .WithMany(m => m.ColaboradorxAtividade)
                 .HasForeignKey(x => x.CXA_IDCOLABORADOR)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
