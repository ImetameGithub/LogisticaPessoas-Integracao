using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Imetame.Documentacao.Infra.Data.Migrations
{
    public partial class InitialFartecSistemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProcessamento_Processamento_ProcessamentoId",
                table: "LogProcessamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Processamento_Pedido_PedidoId",
                table: "Processamento");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoCadastro_Processamento_ProcessamentoId",
                table: "ResultadoCadastro");

            migrationBuilder.DropIndex(
                name: "IX_ResultadoCadastro_ProcessamentoId",
                table: "ResultadoCadastro");

            migrationBuilder.DropIndex(
                name: "IX_Processamento_PedidoId",
                table: "Processamento");

            migrationBuilder.DropIndex(
                name: "IX_LogProcessamento_ProcessamentoId",
                table: "LogProcessamento");

            migrationBuilder.DropColumn(
                name: "ProcessamentoId",
                table: "LogProcessamento");

            migrationBuilder.AlterColumn<string>(
                name: "NumCracha",
                table: "ResultadoCadastro",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumCad",
                table: "ResultadoCadastro",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "ResultadoCadastro",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LogString",
                table: "ResultadoCadastro",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FuncaoAtual",
                table: "ResultadoCadastro",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Equipe",
                table: "ResultadoCadastro",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OssString",
                table: "Processamento",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InicioProcessamento",
                table: "Processamento",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinalProcessamento",
                table: "Processamento",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Unidade",
                table: "Pedido",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumPedido",
                table: "Pedido",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Credenciadora",
                table: "Pedido",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Evento",
                table: "LogProcessamento",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEvento",
                table: "LogProcessamento",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoCadastro_IdProcessamento",
                table: "ResultadoCadastro",
                column: "IdProcessamento");

            migrationBuilder.CreateIndex(
                name: "IX_Processamento_IdPedido",
                table: "Processamento",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_LogProcessamento_IdProcessamento",
                table: "LogProcessamento",
                column: "IdProcessamento");

            migrationBuilder.AddForeignKey(
                name: "FK_LogProcessamento_Processamento_IdProcessamento",
                table: "LogProcessamento",
                column: "IdProcessamento",
                principalTable: "Processamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processamento_Pedido_IdPedido",
                table: "Processamento",
                column: "IdPedido",
                principalTable: "Pedido",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoCadastro_Processamento_IdProcessamento",
                table: "ResultadoCadastro",
                column: "IdProcessamento",
                principalTable: "Processamento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LogProcessamento_Processamento_IdProcessamento",
                table: "LogProcessamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Processamento_Pedido_IdPedido",
                table: "Processamento");

            migrationBuilder.DropForeignKey(
                name: "FK_ResultadoCadastro_Processamento_IdProcessamento",
                table: "ResultadoCadastro");

            migrationBuilder.DropIndex(
                name: "IX_ResultadoCadastro_IdProcessamento",
                table: "ResultadoCadastro");

            migrationBuilder.DropIndex(
                name: "IX_Processamento_IdPedido",
                table: "Processamento");

            migrationBuilder.DropIndex(
                name: "IX_LogProcessamento_IdProcessamento",
                table: "LogProcessamento");

            migrationBuilder.AlterColumn<string>(
                name: "NumCracha",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NumCad",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "LogString",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FuncaoAtual",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Equipe",
                table: "ResultadoCadastro",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OssString",
                table: "Processamento",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InicioProcessamento",
                table: "Processamento",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "FinalProcessamento",
                table: "Processamento",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Unidade",
                table: "Pedido",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "NumPedido",
                table: "Pedido",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Credenciadora",
                table: "Pedido",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Evento",
                table: "LogProcessamento",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataEvento",
                table: "LogProcessamento",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "ProcessamentoId",
                table: "LogProcessamento",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoCadastro_ProcessamentoId",
                table: "ResultadoCadastro",
                column: "ProcessamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Processamento_PedidoId",
                table: "Processamento",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_LogProcessamento_ProcessamentoId",
                table: "LogProcessamento",
                column: "ProcessamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_LogProcessamento_Processamento_ProcessamentoId",
                table: "LogProcessamento",
                column: "ProcessamentoId",
                principalTable: "Processamento",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Processamento_Pedido_PedidoId",
                table: "Processamento",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ResultadoCadastro_Processamento_ProcessamentoId",
                table: "ResultadoCadastro",
                column: "ProcessamentoId",
                principalTable: "Processamento",
                principalColumn: "Id");
        }
    }
}
