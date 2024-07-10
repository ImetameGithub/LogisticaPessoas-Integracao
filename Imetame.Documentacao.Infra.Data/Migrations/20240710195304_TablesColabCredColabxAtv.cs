using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class TablesColabCredColabxAtv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Colaborador",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cracha = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Matricula = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MudaFuncao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colaborador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credenciadora",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credenciadora", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ColaboradorxAtividade",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CXA_IDCOLABORADOR = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CXA_IDATIVIDADE_ESPECIFICA = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColaboradorxAtividade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColaboradorxAtividade_AtividadeEspecifica_CXA_IDATIVIDADE_ESPECIFICA",
                        column: x => x.CXA_IDATIVIDADE_ESPECIFICA,
                        principalTable: "AtividadeEspecifica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColaboradorxAtividade_Colaborador_CXA_IDCOLABORADOR",
                        column: x => x.CXA_IDCOLABORADOR,
                        principalTable: "Colaborador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColaboradorxAtividade_CXA_IDATIVIDADE_ESPECIFICA",
                table: "ColaboradorxAtividade",
                column: "CXA_IDATIVIDADE_ESPECIFICA");

            migrationBuilder.CreateIndex(
                name: "IX_ColaboradorxAtividade_CXA_IDCOLABORADOR",
                table: "ColaboradorxAtividade",
                column: "CXA_IDCOLABORADOR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColaboradorxAtividade");

            migrationBuilder.DropTable(
                name: "Credenciadora");

            migrationBuilder.DropTable(
                name: "Colaborador");
        }
    }
}
