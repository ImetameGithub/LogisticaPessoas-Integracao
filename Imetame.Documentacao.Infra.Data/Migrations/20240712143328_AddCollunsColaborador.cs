using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class AddCollunsColaborador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Codigo_Equipe",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo_Funcao",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo_OS",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome_Equipe",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome_Funcao",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nome_OS",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Perfil",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Codigo_Equipe",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Codigo_Funcao",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Codigo_OS",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Nome_Equipe",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Nome_Funcao",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Nome_OS",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Perfil",
                table: "Colaborador");
        }
    }
}
