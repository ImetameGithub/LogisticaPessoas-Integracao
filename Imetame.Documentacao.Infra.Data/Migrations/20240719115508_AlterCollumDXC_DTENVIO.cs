using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class AlterCollumDXC_DTENVIO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 8, 55, 8, 587, DateTimeKind.Local).AddTicks(1659),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 8, 55, 8, 587, DateTimeKind.Local).AddTicks(1659));
        }
    }
}
