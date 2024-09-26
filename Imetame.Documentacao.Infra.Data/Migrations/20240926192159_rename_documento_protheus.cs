using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class rename_documento_protheus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 195, DateTimeKind.Local).AddTicks(3753),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 218, DateTimeKind.Local).AddTicks(7651));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 196, DateTimeKind.Local).AddTicks(2050),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 219, DateTimeKind.Local).AddTicks(4262));

            migrationBuilder.CreateTable(
                name: "DocumentoXProtheus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdProtheus = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    DescricaoProtheus = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IdDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoXProtheus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoXProtheus_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documento",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DocumentoXProtheus_Documento_IdDocumento",
                        column: x => x.IdDocumento,
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoXProtheus_DocumentoId",
                table: "DocumentoXProtheus",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoXProtheus_IdDocumento",
                table: "DocumentoXProtheus",
                column: "IdDocumento");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoXProtheus");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 218, DateTimeKind.Local).AddTicks(7651),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 195, DateTimeKind.Local).AddTicks(3753));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 13, 28, 49, 219, DateTimeKind.Local).AddTicks(4262),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 16, 21, 59, 196, DateTimeKind.Local).AddTicks(2050));

            migrationBuilder.CreateTable(
                name: "DocumentoProtheus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdDocumento = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DescricaoProtheus = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    IdProtheus = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
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
    }
}
