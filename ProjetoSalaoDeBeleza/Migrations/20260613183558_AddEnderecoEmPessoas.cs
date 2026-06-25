using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AddEnderecoEmPessoas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_oCategoriaCodCategoria",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_oCategoriaCodCategoria",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "oCategoriaCodCategoria",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "Bairro",
                table: "Pessoas",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "Pessoas",
                type: "character varying(8)",
                maxLength: 8,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Pessoas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Pessoas",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Rua",
                table: "Pessoas",
                type: "character varying(35)",
                maxLength: 35,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CodCategoria",
                table: "Produtos",
                column: "CodCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_CodCategoria",
                table: "Produtos",
                column: "CodCategoria",
                principalTable: "Categorias",
                principalColumn: "CodCategoria",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_Categorias_CodCategoria",
                table: "Produtos");

            migrationBuilder.DropIndex(
                name: "IX_Produtos_CodCategoria",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "Bairro",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "CEP",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Rua",
                table: "Pessoas");

            migrationBuilder.AddColumn<int>(
                name: "oCategoriaCodCategoria",
                table: "Produtos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_oCategoriaCodCategoria",
                table: "Produtos",
                column: "oCategoriaCodCategoria");

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_Categorias_oCategoriaCodCategoria",
                table: "Produtos",
                column: "oCategoriaCodCategoria",
                principalTable: "Categorias",
                principalColumn: "CodCategoria");
        }
    }
}
