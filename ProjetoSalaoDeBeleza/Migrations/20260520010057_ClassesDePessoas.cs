using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class ClassesDePessoas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    CodPessoa = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CPF = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    oCidadeCodCidade = table.Column<int>(type: "integer", nullable: false),
                    CodCidade = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    TotalVisitas = table.Column<int>(type: "integer", nullable: true),
                    TotalGasto = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UltimaVisita = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RecebeNotificacoes = table.Column<bool>(type: "boolean", nullable: true),
                    Cargo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Salario = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    DataAdmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataDemissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ComissaoPercentual = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.CodPessoa);
                    table.ForeignKey(
                        name: "FK_Pessoas_Cidades_oCidadeCodCidade",
                        column: x => x.oCidadeCodCidade,
                        principalTable: "Cidades",
                        principalColumn: "CodCidade",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_oCidadeCodCidade",
                table: "Pessoas",
                column: "oCidadeCodCidade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
