using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class correcaodocumento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 17, 11, 32, 880, DateTimeKind.Local).AddTicks(9892),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 195, DateTimeKind.Local).AddTicks(3753));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 17, 11, 32, 882, DateTimeKind.Local).AddTicks(3881),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 196, DateTimeKind.Local).AddTicks(2050));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 195, DateTimeKind.Local).AddTicks(3753),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 17, 11, 32, 880, DateTimeKind.Local).AddTicks(9892));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 196, DateTimeKind.Local).AddTicks(2050),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 17, 11, 32, 882, DateTimeKind.Local).AddTicks(3881));
        }
    }
}
