using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class campoDataEvento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sucesso",
                table: "ResultadoCadastro",
                newName: "Sucesso");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataEvento",
                table: "LogProcessamento",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataEvento",
                table: "LogProcessamento");

            migrationBuilder.RenameColumn(
                name: "Sucesso",
                table: "ResultadoCadastro",
                newName: "sucesso");
        }
    }
}
