using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class alterarNomeCampoFuncao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Funcao",
                table: "ResultadoCadastro",
                newName: "FuncaoAtual");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FuncaoAtual",
                table: "ResultadoCadastro",
                newName: "Funcao");
        }
    }
}
