using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class ConfiguracaoHerancaTPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoas_Cidades_oCidadeCodCidade",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_oCidadeCodCidade",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "oCidadeCodCidade",
                table: "Pessoas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimaVisita",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Pessoas",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Pessoas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDemissao",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAdmissao",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_CodCidade",
                table: "Pessoas",
                column: "CodCidade");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoas_Cidades_CodCidade",
                table: "Pessoas",
                column: "CodCidade",
                principalTable: "Cidades",
                principalColumn: "CodCidade",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pessoas_Cidades_CodCidade",
                table: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_Pessoas_CodCidade",
                table: "Pessoas");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimaVisita",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefone",
                table: "Pessoas",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Pessoas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataDemissao",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataAdmissao",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "oCidadeCodCidade",
                table: "Pessoas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_oCidadeCodCidade",
                table: "Pessoas",
                column: "oCidadeCodCidade");

            migrationBuilder.AddForeignKey(
                name: "FK_Pessoas_Cidades_oCidadeCodCidade",
                table: "Pessoas",
                column: "oCidadeCodCidade",
                principalTable: "Cidades",
                principalColumn: "CodCidade",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
