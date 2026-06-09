using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCondicaoPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CondicoesPagamento",
                columns: table => new
                {
                    CodCondicao = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descricao = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeroParcelas = table.Column<int>(type: "integer", nullable: false),
                    TaxaJuros = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    Desconto = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PrazoDias = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicoesPagamento", x => x.CodCondicao);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CondicoesPagamento");
        }
    }
}
