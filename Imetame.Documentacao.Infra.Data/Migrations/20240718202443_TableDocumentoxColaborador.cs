using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class TableDocumentoxColaborador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentoxColaborador",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DXC_CODPROTHEUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DXC_CODDESTRA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DXC_BASE64 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DXC_IDCOLABORADOR = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoxColaborador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoxColaborador_Colaborador_DXC_IDCOLABORADOR",
                        column: x => x.DXC_IDCOLABORADOR,
                        principalTable: "Colaborador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoxColaborador_DXC_IDCOLABORADOR",
                table: "DocumentoxColaborador",
                column: "DXC_IDCOLABORADOR");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoxColaborador");
        }
    }
}
