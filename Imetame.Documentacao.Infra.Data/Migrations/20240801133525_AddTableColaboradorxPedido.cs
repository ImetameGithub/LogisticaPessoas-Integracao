using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class AddTableColaboradorxPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(1427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 1, 10, 29, 22, 924, DateTimeKind.Local).AddTicks(8503));

            migrationBuilder.CreateTable(
                name: "ColaboradorxPedido",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CXP_NUMEROOS = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    CXP_USUARIOINCLUSAO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CXP_DTINCLUSAO = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(5079)),
                    CXP_IDCOLABORADOR = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CXP_IDPEDIDO = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColaboradorxPedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColaboradorxPedido_Colaborador_CXP_IDCOLABORADOR",
                        column: x => x.CXP_IDCOLABORADOR,
                        principalTable: "Colaborador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColaboradorxPedido_Pedido_CXP_IDPEDIDO",
                        column: x => x.CXP_IDPEDIDO,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColaboradorxPedido_CXP_IDCOLABORADOR",
                table: "ColaboradorxPedido",
                column: "CXP_IDCOLABORADOR");

            migrationBuilder.CreateIndex(
                name: "IX_ColaboradorxPedido_CXP_IDPEDIDO",
                table: "ColaboradorxPedido",
                column: "CXP_IDPEDIDO");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColaboradorxPedido");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 1, 10, 29, 22, 924, DateTimeKind.Local).AddTicks(8503),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(1427));
        }
    }
}
