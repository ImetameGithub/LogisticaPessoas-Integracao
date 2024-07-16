﻿// <auto-generated />
using System;
using Imetame.Documentacao.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.AtividadeEspecifica", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdDestra")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AtividadeEspecifica", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Colaborador", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Codigo_Disciplina")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Codigo_Equipe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Codigo_Funcao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Codigo_OS")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cracha")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Matricula")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("MudaFuncao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome_Disciplina")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome_Equipe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome_Funcao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome_OS")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Perfil")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SincronizadoDestra")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("Colaborador", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.ColaboradorxAtividade", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CXA_IDATIVIDADE_ESPECIFICA")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CXA_IDCOLABORADOR")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CXA_IDATIVIDADE_ESPECIFICA");

                    b.HasIndex("CXA_IDCOLABORADOR");

                    b.ToTable("ColaboradorxAtividade", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Credenciadora", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Credenciadora", (string)null);
                });

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

                    b.Property<DateTime>("DataEvento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Evento")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<Guid>("IdProcessamento")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("IdProcessamento");

                    b.ToTable("LogProcessamento", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Pedido", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Credenciadora")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NumPedido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Unidade")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Pedido", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Processamento", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("FinalProcessamento")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("IdPedido")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("InicioProcessamento")
                        .HasColumnType("datetime2");

                    b.Property<string>("OssString")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdPedido");

                    b.ToTable("Processamento", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.ResultadoCadastro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Equipe")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FuncaoAtual")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid>("IdProcessamento")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LogString")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("NumCad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NumCracha")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Sucesso")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IdProcessamento");

                    b.ToTable("ResultadoCadastro", (string)null);
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.ColaboradorxAtividade", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.AtividadeEspecifica", "AtividadeEspecifica")
                        .WithMany()
                        .HasForeignKey("CXA_IDATIVIDADE_ESPECIFICA")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Imetame.Documentacao.Domain.Entities.Colaborador", "Colaborador")
                        .WithMany("ColaboradorxAtividade")
                        .HasForeignKey("CXA_IDCOLABORADOR")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AtividadeEspecifica");

                    b.Navigation("Colaborador");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.LogProcessamento", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.Processamento", "Processamento")
                        .WithMany()
                        .HasForeignKey("IdProcessamento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Processamento");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Processamento", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.Pedido", "Pedido")
                        .WithMany()
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.ResultadoCadastro", b =>
                {
                    b.HasOne("Imetame.Documentacao.Domain.Entities.Processamento", "Processamento")
                        .WithMany()
                        .HasForeignKey("IdProcessamento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Processamento");
                });

            modelBuilder.Entity("Imetame.Documentacao.Domain.Entities.Colaborador", b =>
                {
                    b.Navigation("ColaboradorxAtividade");
                });
#pragma warning restore 612, 618
        }
    }
}
