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
    internal class AtividadeEspecificaMap : BaseMap<AtividadeEspecifica>
    {
        public override void ConfigureEntity(EntityTypeBuilder<AtividadeEspecifica> builder)
        {
            builder.ToTable(nameof(AtividadeEspecifica));

            builder.Property(c => c.Descricao).IsRequired();
            builder.Property(c => c.Codigo).IsRequired();
            builder.Property(c => c.Id).IsRequired();
        }
    }
}
