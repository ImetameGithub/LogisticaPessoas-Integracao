using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class DeleteCollunsPedidoIdeProcessamentoId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessamentoId",
                table: "ResultadoCadastro");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Processamento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProcessamentoId",
                table: "ResultadoCadastro",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PedidoId",
                table: "Processamento",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
