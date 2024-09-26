using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class alteracao_documentoprotheus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoProtheus",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "IdProtheus",
                table: "Documento");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 218, DateTimeKind.Local).AddTicks(7651),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 652, DateTimeKind.Local).AddTicks(6775));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 219, DateTimeKind.Local).AddTicks(4262),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 653, DateTimeKind.Local).AddTicks(8032));

            migrationBuilder.CreateTable(
                name: "DocumentoProtheus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProtheus = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    DescricaoProtheus = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IdDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoProtheus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoProtheus_Documento_IdDocumento",
                        column: x => x.IdDocumento,
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoProtheus_IdDocumento",
                table: "DocumentoProtheus",
                column: "IdDocumento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoProtheus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 652, DateTimeKind.Local).AddTicks(6775),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 218, DateTimeKind.Local).AddTicks(7651));

            migrationBuilder.AddColumn<string>(
                name: "DescricaoProtheus",
                table: "Documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdProtheus",
                table: "Documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 653, DateTimeKind.Local).AddTicks(8032),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 219, DateTimeKind.Local).AddTicks(4262));
        }
    }
}
