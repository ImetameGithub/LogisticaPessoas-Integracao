using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class camposResultado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cracha",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Equipe",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Funcao",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cracha",
                table: "ResultadoCadastro");

            migrationBuilder.DropColumn(
                name: "Equipe",
                table: "ResultadoCadastro");

            migrationBuilder.DropColumn(
                name: "Funcao",
                table: "ResultadoCadastro");
        }
    }
}
