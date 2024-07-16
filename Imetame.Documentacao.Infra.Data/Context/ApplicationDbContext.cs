using Imetame.Documentacao.Domain.Core.Interfaces;
using Imetame.Documentacao.Domain.Core.Models;
using Imetame.Documentacao.Domain.Entities;
using Imetame.Documentacao.Domain.Models;
using Imetame.Documentacao.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imetame.Documentacao.Infra.Data.Context
{
    public class ApplicationDbContext : DbContext
    {


        public DbSet<Domain.Entities.CredenciadoraDePara> CredenciadoraDeParas { get; set; }
        public DbSet<Domain.Entities.LogProcessamento> LogProcessamento { get; set; }
        public DbSet<Domain.Entities.Credenciadora> Credenciadora { get; set; }
        public DbSet<Domain.Entities.Pedido> Pedido { get; set; }
        public DbSet<Domain.Entities.Processamento> Processamento { get; set; }
        public DbSet<Domain.Entities.ResultadoCadastro> ResultadoCadastro { get; set; }
        public DbSet<Domain.Entities.AtividadeEspecifica> AtividadeEspecifica { get; set; }
        public DbSet<Domain.Entities.Documento> Documento { get; set; }
        public DbSet<Domain.Entities.Colaborador> Colaborador { get; set; }
        public DbSet<Domain.Entities.ColaboradorxAtividade> ColaboradorxAtividade { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    if (!typeof(IEntity).IsAssignableFrom(entityType.ClrType)) continue;

            //    foreach (var property in entityType.GetProperties())
            //    {
            //        if (property.ClrType == typeof(decimal) || property.ClrType == typeof(decimal?))
            //            property.SetColumnType("decimal(18, 2)");
            //        if (property.ClrType == typeof(string))
            //        {
            //            property.SetColumnType("varchar(256)");
            //            property.SetMaxLength(256);
            //        }
            //        if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
            //            property.SetColumnType("datetime");
            //    }
            //}

            base.OnModelCreating(modelBuilder);


            // MATHEUS FARTEC SISTEMAS - CONFIGURAÇÃO PARA OS MAP SEREM USADOS
            modelBuilder.Entity<Domain.Entities.ResultadoCadastro>(new ResultadoCadastroMap().Configure);
            modelBuilder.Entity<Domain.Entities.Credenciadora>(new CredenciadoraMap().Configure);
            modelBuilder.Entity<Domain.Entities.CredenciadoraDePara>(new CredenciadoraDeParaMap().Configure);
            modelBuilder.Entity<Domain.Entities.LogProcessamento>(new LogProcessamentoMap().Configure);
            modelBuilder.Entity<Domain.Entities.Pedido>(new PedidoMap().Configure);        
            modelBuilder.Entity<Domain.Entities.Documento>(new DocumentoMap().Configure);        
            modelBuilder.Entity<Domain.Entities.Processamento>(new ProcessamentoMap().Configure);
            modelBuilder.Entity<Domain.Entities.AtividadeEspecifica>(new AtividadeEspecificaMap().Configure);
            modelBuilder.Entity<Domain.Entities.Colaborador>(new ColaboradorMap().Configure);
            modelBuilder.Entity<Domain.Entities.ColaboradorxAtividade>(new ColaboradorxAtividadeMap().Configure);




            modelBuilder.ApplyConfiguration(new CredenciadoraDeParaMap());
            



        }



    }
}
