using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriasProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas");

            migrationBuilder.AlterColumn<int>(
                name: "oCondicaoCodCondicao",
                table: "CondicoesPagamentoParcelas",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CodCategoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Categoria = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CodCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    CodProduto = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Produto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PrecoCusto = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    PrecoVenda = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Estoque = table.Column<int>(type: "integer", nullable: false),
                    UnidadeMedida = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    CodCategoria = table.Column<int>(type: "integer", nullable: false),
                    oCategoriaCodCategoria = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.CodProduto);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_oCategoriaCodCategoria",
                        column: x => x.oCategoriaCodCategoria,
                        principalTable: "Categorias",
                        principalColumn: "CodCategoria");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_oCategoriaCodCategoria",
                table: "Produtos",
                column: "oCategoriaCodCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas",
                column: "oCondicaoCodCondicao",
                principalTable: "CondicoesPagamento",
                principalColumn: "CodCondicao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.AlterColumn<int>(
                name: "oCondicaoCodCondicao",
                table: "CondicoesPagamentoParcelas",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                table: "CondicoesPagamentoParcelas",
                column: "oCondicaoCodCondicao",
                principalTable: "CondicoesPagamento",
                principalColumn: "CodCondicao",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
