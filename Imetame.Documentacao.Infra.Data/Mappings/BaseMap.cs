using Imetame.Documentacao.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Imetame.Documentacao.Infra.Data.Mappings
{
    public abstract class BaseMap<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            

            builder.HasKey(c => c.Id);

            ConfigureEntity(builder);
        }

        public abstract void ConfigureEntity(EntityTypeBuilder<T> builder);


    }
}
