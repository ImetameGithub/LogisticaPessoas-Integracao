using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class update_tbl_documento_new_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdDestra",
                table: "Documento",
                newName: "IdDocCredenciadora");

            migrationBuilder.RenameColumn(
                name: "DescricaoDestra",
                table: "Documento",
                newName: "DescricaoCredenciadora");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 5, 9, 37, 54, 214, DateTimeKind.Local).AddTicks(7966),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 652, DateTimeKind.Local).AddTicks(6775));

            migrationBuilder.AddColumn<Guid>(
                name: "IdCredenciadora",
                table: "Documento",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 5, 9, 37, 54, 215, DateTimeKind.Local).AddTicks(2387),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 653, DateTimeKind.Local).AddTicks(8032));

            migrationBuilder.CreateIndex(
                name: "IX_Documento_IdCredenciadora",
                table: "Documento",
                column: "IdCredenciadora");

            migrationBuilder.AddForeignKey(
                name: "FK_Documento_Credenciadora_IdCredenciadora",
                table: "Documento",
                column: "IdCredenciadora",
                principalTable: "Credenciadora",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documento_Credenciadora_IdCredenciadora",
                table: "Documento");

            migrationBuilder.DropIndex(
                name: "IX_Documento_IdCredenciadora",
                table: "Documento");

            migrationBuilder.DropColumn(
                name: "IdCredenciadora",
                table: "Documento");

            migrationBuilder.RenameColumn(
                name: "IdDocCredenciadora",
                table: "Documento",
                newName: "IdDestra");

            migrationBuilder.RenameColumn(
                name: "DescricaoCredenciadora",
                table: "Documento",
                newName: "DescricaoDestra");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DXC_DTENVIO",
                table: "DocumentoxColaborador",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 652, DateTimeKind.Local).AddTicks(6775),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 5, 9, 37, 54, 214, DateTimeKind.Local).AddTicks(7966));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CXP_DTINCLUSAO",
                table: "ColaboradorxPedido",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 8, 27, 13, 23, 46, 653, DateTimeKind.Local).AddTicks(8032),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 5, 9, 37, 54, 215, DateTimeKind.Local).AddTicks(2387));
        }
    }
}
