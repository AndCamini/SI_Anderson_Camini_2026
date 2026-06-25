using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class AddFornecedoresTransportadoresVeiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fornecedores",
                columns: table => new
                {
                    CodFornecedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RazaoSocial = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    NomeFantasia = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CNPJ = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    InscricaoEstadual = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CEP = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    Rua = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CodCidade = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedores", x => x.CodFornecedor);
                    table.ForeignKey(
                        name: "FK_Fornecedores_Cidades_CodCidade",
                        column: x => x.CodCidade,
                        principalTable: "Cidades",
                        principalColumn: "CodCidade",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarcasVeiculos",
                columns: table => new
                {
                    CodMarca = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MarcaVeiculo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarcasVeiculos", x => x.CodMarca);
                });

            migrationBuilder.CreateTable(
                name: "TiposVeiculos",
                columns: table => new
                {
                    CodTipo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposVeiculos", x => x.CodTipo);
                });

            migrationBuilder.CreateTable(
                name: "Transportadores",
                columns: table => new
                {
                    CodTransportador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CPF = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    CNPJ = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    CEP = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    Rua = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Numero = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Complemento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Bairro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CodCidade = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transportadores", x => x.CodTransportador);
                    table.ForeignKey(
                        name: "FK_Transportadores_Cidades_CodCidade",
                        column: x => x.CodCidade,
                        principalTable: "Cidades",
                        principalColumn: "CodCidade",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    CodVeiculo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Placa = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    PlacaMercosul = table.Column<bool>(type: "boolean", nullable: false),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Ano = table.Column<int>(type: "integer", nullable: false),
                    CodMarca = table.Column<int>(type: "integer", nullable: false),
                    CodTipo = table.Column<int>(type: "integer", nullable: false),
                    CodTransportador = table.Column<int>(type: "integer", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.CodVeiculo);
                    table.ForeignKey(
                        name: "FK_Veiculos_MarcasVeiculos_CodMarca",
                        column: x => x.CodMarca,
                        principalTable: "MarcasVeiculos",
                        principalColumn: "CodMarca",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Veiculos_TiposVeiculos_CodTipo",
                        column: x => x.CodTipo,
                        principalTable: "TiposVeiculos",
                        principalColumn: "CodTipo",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Veiculos_Transportadores_CodTransportador",
                        column: x => x.CodTransportador,
                        principalTable: "Transportadores",
                        principalColumn: "CodTransportador",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedores_CodCidade",
                table: "Fornecedores",
                column: "CodCidade");

            migrationBuilder.CreateIndex(
                name: "IX_Transportadores_CodCidade",
                table: "Transportadores",
                column: "CodCidade");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_CodMarca",
                table: "Veiculos",
                column: "CodMarca");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_CodTipo",
                table: "Veiculos",
                column: "CodTipo");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_CodTransportador",
                table: "Veiculos",
                column: "CodTransportador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fornecedores");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "MarcasVeiculos");

            migrationBuilder.DropTable(
                name: "TiposVeiculos");

            migrationBuilder.DropTable(
                name: "Transportadores");
        }
    }
}
