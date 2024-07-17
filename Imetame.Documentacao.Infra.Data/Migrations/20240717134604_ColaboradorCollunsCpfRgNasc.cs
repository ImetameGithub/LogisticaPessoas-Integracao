using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class ColaboradorCollunsCpfRgNasc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdProtheus",
                table: "Documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdDestra",
                table: "Documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DataAdmissao",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nascimento",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rg",
                table: "Colaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "DataAdmissao",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Nascimento",
                table: "Colaborador");

            migrationBuilder.DropColumn(
                name: "Rg",
                table: "Colaborador");

            migrationBuilder.AlterColumn<string>(
                name: "IdProtheus",
                table: "Documento",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "IdDestra",
                table: "Documento",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);
        }
    }
}
