using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class addCollunsDocumento : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 13, 27, 27, 846, DateTimeKind.Local).AddTicks(8664),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 8, 55, 8, 587, DateTimeKind.Local).AddTicks(1659));

            migrationBuilder.AddColumn<string>(
                name: "DXC_DESCDESTRA",
                table: "DocumentoxColaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DXC_DESCPROTHEUS",
                table: "DocumentoxColaborador",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DXC_DESCDESTRA",
                table: "DocumentoxColaborador");

            migrationBuilder.DropColumn(
                name: "DXC_DESCPROTHEUS",
                table: "DocumentoxColaborador");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 7, 19, 8, 55, 8, 587, DateTimeKind.Local).AddTicks(1659),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 7, 19, 13, 27, 27, 846, DateTimeKind.Local).AddTicks(8664));
        }
    }
}
