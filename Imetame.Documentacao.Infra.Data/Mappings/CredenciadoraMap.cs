using Imetame.Documentacao.Domain.Core.Models;
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
    internal class CredenciadoraMap : BaseMap<Credenciadora>
    {
        public override void ConfigureEntity(EntityTypeBuilder<Credenciadora> builder)
        {
            builder.ToTable(nameof(Credenciadora));

            builder.Property(c => c.Descricao).HasColumnType("varchar(255)").HasMaxLength(255).IsRequired();            
        }
    }
}
