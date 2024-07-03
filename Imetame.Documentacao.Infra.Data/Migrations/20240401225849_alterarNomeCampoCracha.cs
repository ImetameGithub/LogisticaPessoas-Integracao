using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class alterarNomeCampoCracha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cracha",
                table: "ResultadoCadastro",
                newName: "NumCracha");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumCracha",
                table: "ResultadoCadastro",
                newName: "Cracha");
        }
    }
}
