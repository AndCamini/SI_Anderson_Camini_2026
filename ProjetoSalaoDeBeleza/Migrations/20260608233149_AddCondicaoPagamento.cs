using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AddCondicaoPagamento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxaJuros",
                table: "CondicoesPagamento",
                newName: "Multa");

            migrationBuilder.RenameColumn(
                name: "PrazoDias",
                table: "CondicoesPagamento",
                newName: "PrimeiraParcela");

            migrationBuilder.AddColumn<int>(
                name: "EntreParcelas",
                table: "CondicoesPagamento",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Juros",
                table: "CondicoesPagamento",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "CondicoesPagamentoParcelas",
                columns: table => new
                {
                    CodParcela = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodCondicao = table.Column<int>(type: "integer", nullable: false),
                    oCondicaoCodCondicao = table.Column<int>(type: "integer", nullable: false),
                    Numero = table.Column<int>(type: "integer", nullable: false),
                    Dias = table.Column<int>(type: "integer", nullable: false),
                    Percentual = table.Column<decimal>(type: "numeric(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CondicoesPagamentoParcelas", x => x.CodParcela);
                    table.ForeignKey(
                        name: "FK_CondicoesPagamentoParcelas_CondicoesPagamento_oCondicaoCodC~",
                        column: x => x.oCondicaoCodCondicao,
                        principalTable: "CondicoesPagamento",
                        principalColumn: "CodCondicao",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CondicoesPagamentoParcelas_oCondicaoCodCondicao",
                table: "CondicoesPagamentoParcelas",
                column: "oCondicaoCodCondicao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CondicoesPagamentoParcelas");

            migrationBuilder.DropColumn(
                name: "EntreParcelas",
                table: "CondicoesPagamento");

            migrationBuilder.DropColumn(
                name: "Juros",
                table: "CondicoesPagamento");

            migrationBuilder.RenameColumn(
                name: "PrimeiraParcela",
                table: "CondicoesPagamento",
                newName: "PrazoDias");

            migrationBuilder.RenameColumn(
                name: "Multa",
                table: "CondicoesPagamento",
                newName: "TaxaJuros");
        }
    }
}
