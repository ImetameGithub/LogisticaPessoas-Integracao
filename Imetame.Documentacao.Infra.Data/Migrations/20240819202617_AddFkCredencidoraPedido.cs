using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class AddFkCredencidoraPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credenciadora",
                table: "Pedido");

            migrationBuilder.AddColumn<Guid>(
                name: "IdCredenciadora",
                table: "Pedido",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 19, 17, 26, 17, 363, DateTimeKind.Local).AddTicks(6374),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(1427));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 19, 17, 26, 17, 363, DateTimeKind.Local).AddTicks(9680),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(5079));

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_IdCredenciadora",
                table: "Pedido",
                column: "IdCredenciadora");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Credenciadora_IdCredenciadora",
                table: "Pedido",
                column: "IdCredenciadora",
                principalTable: "Credenciadora",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Credenciadora_IdCredenciadora",
                table: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_Pedido_IdCredenciadora",
                table: "Pedido");

            migrationBuilder.DropColumn(
                name: "IdCredenciadora",
                table: "Pedido");

            migrationBuilder.AddColumn<string>(
                name: "Credenciadora",
                table: "Pedido",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(1427),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 19, 17, 26, 17, 363, DateTimeKind.Local).AddTicks(6374));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 1, 10, 35, 25, 497, DateTimeKind.Local).AddTicks(5079),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 19, 17, 26, 17, 363, DateTimeKind.Local).AddTicks(9680));
        }
    }
}
