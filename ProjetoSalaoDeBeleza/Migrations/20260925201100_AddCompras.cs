using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AddCompras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    CodCompra = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Modelo = table.Column<int>(type: "integer", nullable: false),
                    Serie = table.Column<int>(type: "integer", nullable: false),
                    NumeroNota = table.Column<int>(type: "integer", nullable: true),
                    CodFornecedor = table.Column<int>(type: "integer", nullable: false),
                    CodTransportador = table.Column<int>(type: "integer", nullable: true),
                    DataEmissao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataChegada = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Frete = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Seguro = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    OutrasDespesas = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    CodCondicaoPagamento = table.Column<int>(type: "integer", nullable: true),
                    CodFormaPagamento = table.Column<int>(type: "integer", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UsuarioUltimaAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compras", x => x.CodCompra);
                    table.ForeignKey(
                        name: "FK_Compras_CondicoesPagamento_CodCondicaoPagamento",
                        column: x => x.CodCondicaoPagamento,
                        principalTable: "CondicoesPagamento",
                        principalColumn: "CodCondicao",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_FormasPagamento_CodFormaPagamento",
                        column: x => x.CodFormaPagamento,
                        principalTable: "FormasPagamento",
                        principalColumn: "CodFormaPagamento",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Fornecedores_CodFornecedor",
                        column: x => x.CodFornecedor,
                        principalTable: "Fornecedores",
                        principalColumn: "CodFornecedor",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compras_Transportadores_CodTransportador",
                        column: x => x.CodTransportador,
                        principalTable: "Transportadores",
                        principalColumn: "CodTransportador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprasItens",
                columns: table => new
                {
                    CodItem = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodCompra = table.Column<int>(type: "integer", nullable: false),
                    CodProduto = table.Column<int>(type: "integer", nullable: false),
                    Quantidade = table.Column<int>(type: "integer", nullable: false),
                    PrecoUnitario = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasItens", x => x.CodItem);
                    table.ForeignKey(
                        name: "FK_ComprasItens_Compras_CodCompra",
                        column: x => x.CodCompra,
                        principalTable: "Compras",
                        principalColumn: "CodCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComprasItens_Produtos_CodProduto",
                        column: x => x.CodProduto,
                        principalTable: "Produtos",
                        principalColumn: "CodProduto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComprasParcelas",
                columns: table => new
                {
                    CodParcela = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodCompra = table.Column<int>(type: "integer", nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    Vencimento = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprasParcelas", x => x.CodParcela);
                    table.ForeignKey(
                        name: "FK_ComprasParcelas_Compras_CodCompra",
                        column: x => x.CodCompra,
                        principalTable: "Compras",
                        principalColumn: "CodCompra",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CodCondicaoPagamento",
                table: "Compras",
                column: "CodCondicaoPagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CodFormaPagamento",
                table: "Compras",
                column: "CodFormaPagamento");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CodFornecedor",
                table: "Compras",
                column: "CodFornecedor");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_CodTransportador",
                table: "Compras",
                column: "CodTransportador");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasItens_CodCompra",
                table: "ComprasItens",
                column: "CodCompra");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasItens_CodProduto",
                table: "ComprasItens",
                column: "CodProduto");

            migrationBuilder.CreateIndex(
                name: "IX_ComprasParcelas_CodCompra",
                table: "ComprasParcelas",
                column: "CodCompra");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComprasItens");

            migrationBuilder.DropTable(
                name: "ComprasParcelas");

            migrationBuilder.DropTable(
                name: "Compras");
        }
    }
}
