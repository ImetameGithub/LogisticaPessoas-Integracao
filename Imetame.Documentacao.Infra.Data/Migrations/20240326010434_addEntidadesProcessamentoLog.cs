using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class addEntidadesProcessamentoLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumPedido = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Unidade = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Credenciadora = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Processamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdPedido = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PedidoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OssString = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    InicioProcessamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    FinalProcessamento = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Processamento_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LogProcessamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProcessamento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Evento = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogProcessamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogProcessamento_Processamento_ProcessamentoId",
                        column: x => x.ProcessamentoId,
                        principalTable: "Processamento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResultadoCadastro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProcessamento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessamentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    sucesso = table.Column<bool>(type: "bit", nullable: false),
                    NumCad = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Nome = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LogString = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoCadastro", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadoCadastro_Processamento_ProcessamentoId",
                        column: x => x.ProcessamentoId,
                        principalTable: "Processamento",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogProcessamento_ProcessamentoId",
                table: "LogProcessamento",
                column: "ProcessamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processamento_PedidoId",
                table: "Processamento",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoCadastro_ProcessamentoId",
                table: "ResultadoCadastro",
                column: "ProcessamentoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogProcessamento");

            migrationBuilder.DropTable(
                name: "ResultadoCadastro");

            migrationBuilder.DropTable(
                name: "Processamento");

            migrationBuilder.DropTable(
                name: "Pedido");
        }
    }
}
