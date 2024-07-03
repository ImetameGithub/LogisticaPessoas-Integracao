﻿// <auto-generated />
using System;
using Imetame.Documentacao.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240329005918_camposLogs")]
    partial class camposLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.CredenciadoraDePara", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Credenciadora")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("De")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Para")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("CredenciadoraDePara", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.LogProcessamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Evento")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<Guid>("IdProcessamento")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ProcessamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProcessamentoId");

                    b.ToTable("LogProcessamento");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Credenciadora")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NumPedido")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Unidade")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Pedido");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Processamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("FinalProcessamento")
                        .HasColumnType("datetime");

                    b.Property<Guid>("IdPedido")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("InicioProcessamento")
                        .HasColumnType("datetime");

                    b.Property<string>("OssString")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<Guid?>("PedidoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PedidoId");

                    b.ToTable("Processamento");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.ResultadoCadastro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdProcessamento")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LogString")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("Nome")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NumCad")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<Guid?>("ProcessamentoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("sucesso")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ProcessamentoId");

                    b.ToTable("ResultadoCadastro");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.LogProcessamento", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.Processamento", "Processamento")
                        .WithMany()
                        .HasForeignKey("ProcessamentoId");

                    b.Navigation("Processamento");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Processamento", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.Pedido", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoId");

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.ResultadoCadastro", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.Processamento", "Processamento")
                        .WithMany()
                        .HasForeignKey("ProcessamentoId");

                    b.Navigation("Processamento");
                });
#pragma warning restore 612, 618
        }
    }
}
