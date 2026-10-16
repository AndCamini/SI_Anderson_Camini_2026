using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjetoSalaoDeBeleza.Migrations
{
    /// <inheritdoc />
    public partial class Agenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agendamentos",
                columns: table => new
                {
                    CodAgendamento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodCliente = table.Column<int>(type: "integer", nullable: false),
                    CodFuncionario = table.Column<int>(type: "integer", nullable: false),
                    CodServico = table.Column<int>(type: "integer", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Duracao = table.Column<int>(type: "integer", nullable: false),
                    Valor = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    UsuarioUltimaAlteracao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agendamentos", x => x.CodAgendamento);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Pessoas_CodCliente",
                        column: x => x.CodCliente,
                        principalTable: "Pessoas",
                        principalColumn: "CodPessoa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Pessoas_CodFuncionario",
                        column: x => x.CodFuncionario,
                        principalTable: "Pessoas",
                        principalColumn: "CodPessoa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Agendamentos_Servicos_CodServico",
                        column: x => x.CodServico,
                        principalTable: "Servicos",
                        principalColumn: "CodServico",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_CodCliente",
                table: "Agendamentos",
                column: "CodCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_CodFuncionario_DataHoraInicio",
                table: "Agendamentos",
                columns: new[] { "CodFuncionario", "DataHoraInicio" });

            migrationBuilder.CreateIndex(
                name: "IX_Agendamentos_CodServico",
                table: "Agendamentos",
                column: "CodServico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Agendamentos");
        }
    }
}
