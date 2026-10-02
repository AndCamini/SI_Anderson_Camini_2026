using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AjustesCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComprasItens_Compras_CodCompra",
                table: "ComprasItens");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasParcelas_Compras_CodCompra",
                table: "ComprasParcelas");

            migrationBuilder.DropIndex(
                name: "IX_ComprasParcelas_CodCompra",
                table: "ComprasParcelas");

            migrationBuilder.DropIndex(
                name: "IX_ComprasItens_CodCompra",
                table: "ComprasItens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compras",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "CodCompra",
                table: "Compras");

            migrationBuilder.RenameColumn(
                name: "CodCompra",
                table: "ComprasParcelas",
                newName: "Serie");

            migrationBuilder.RenameColumn(
                name: "CodCompra",
                table: "ComprasItens",
                newName: "Serie");

            migrationBuilder.AddColumn<int>(
                name: "CodFornecedor",
                table: "ComprasParcelas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Modelo",
                table: "ComprasParcelas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumeroNota",
                table: "ComprasParcelas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CodFornecedor",
                table: "ComprasItens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Modelo",
                table: "ComprasItens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumeroNota",
                table: "ComprasItens",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Compras",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumeroNota",
                table: "Compras",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compras",
                table: "Compras",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" });

            migrationBuilder.CreateIndex(
                name: "IX_ComprasParcelas_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasParcelas",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" });

            migrationBuilder.CreateIndex(
                name: "IX_ComprasItens_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" });

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasItens_Compras_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                principalTable: "Compras",
                principalColumns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasParcelas_Compras_Modelo_Serie_NumeroNota_CodForneced~",
                table: "ComprasParcelas",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                principalTable: "Compras",
                principalColumns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComprasItens_Compras_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasParcelas_Compras_Modelo_Serie_NumeroNota_CodForneced~",
                table: "ComprasParcelas");

            migrationBuilder.DropIndex(
                name: "IX_ComprasParcelas_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasParcelas");

            migrationBuilder.DropIndex(
                name: "IX_ComprasItens_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compras",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "CodFornecedor",
                table: "ComprasParcelas");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "ComprasParcelas");

            migrationBuilder.DropColumn(
                name: "NumeroNota",
                table: "ComprasParcelas");

            migrationBuilder.DropColumn(
                name: "CodFornecedor",
                table: "ComprasItens");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "ComprasItens");

            migrationBuilder.DropColumn(
                name: "NumeroNota",
                table: "ComprasItens");

            migrationBuilder.RenameColumn(
                name: "Serie",
                table: "ComprasParcelas",
                newName: "CodCompra");

            migrationBuilder.RenameColumn(
                name: "Serie",
                table: "ComprasItens",
                newName: "CodCompra");

            migrationBuilder.AlterColumn<string>(
                name: "Observacoes",
                table: "Compras",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumeroNota",
                table: "Compras",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "CodCompra",
                table: "Compras",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compras",
                table: "Compras",
                column: "CodCompra");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasParcelas_CodCompra",
                table: "ComprasParcelas",
                column: "CodCompra");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasItens_CodCompra",
                table: "ComprasItens",
                column: "CodCompra");

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasItens_Compras_CodCompra",
                table: "ComprasItens",
                column: "CodCompra",
                principalTable: "Compras",
                principalColumn: "CodCompra",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasParcelas_Compras_CodCompra",
                table: "ComprasParcelas",
                column: "CodCompra",
                principalTable: "Compras",
                principalColumn: "CodCompra",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
