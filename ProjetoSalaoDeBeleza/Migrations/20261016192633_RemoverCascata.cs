using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class RemoverCascata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cidades_Estados_CodEstado",
                table: "Cidades");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasItens_Compras_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasParcelas_Compras_Modelo_Serie_NumeroNota_CodForneced~",
                table: "ComprasParcelas");

            migrationBuilder.DropForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas");

            migrationBuilder.DropForeignKey(
                name: "FK_Estados_Paises_CodPais",
                table: "Estados");

            migrationBuilder.AddForeignKey(
                name: "FK_Cidades_Estados_CodEstado",
                table: "Cidades",
                column: "CodEstado",
                principalTable: "Estados",
                principalColumn: "CodEstado",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasItens_Compras_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                principalTable: "Compras",
                principalColumns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComprasParcelas_Compras_Modelo_Serie_NumeroNota_CodForneced~",
                table: "ComprasParcelas",
                columns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                principalTable: "Compras",
                principalColumns: new[] { "Modelo", "Serie", "NumeroNota", "CodFornecedor" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas",
                column: "oCondicaoCodCondicao",
                principalTable: "CondicoesPagamento",
                principalColumn: "CodCondicao",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estados_Paises_CodPais",
                table: "Estados",
                column: "CodPais",
                principalTable: "Paises",
                principalColumn: "CodPais",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cidades_Estados_CodEstado",
                table: "Cidades");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasItens_Compras_Modelo_Serie_NumeroNota_CodFornecedor",
                table: "ComprasItens");

            migrationBuilder.DropForeignKey(
                name: "FK_ComprasParcelas_Compras_Modelo_Serie_NumeroNota_CodForneced~",
                table: "ComprasParcelas");

            migrationBuilder.DropForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas");

            migrationBuilder.DropForeignKey(
                name: "FK_Estados_Paises_CodPais",
                table: "Estados");

            migrationBuilder.AddForeignKey(
                name: "FK_Cidades_Estados_CodEstado",
                table: "Cidades",
                column: "CodEstado",
                principalTable: "Estados",
                principalColumn: "CodEstado",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas",
                column: "oCondicaoCodCondicao",
                principalTable: "CondicoesPagamento",
                principalColumn: "CodCondicao");

            migrationBuilder.AddForeignKey(
                name: "FK_Estados_Paises_CodPais",
                table: "Estados",
                column: "CodPais",
                principalTable: "Paises",
                principalColumn: "CodPais",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
