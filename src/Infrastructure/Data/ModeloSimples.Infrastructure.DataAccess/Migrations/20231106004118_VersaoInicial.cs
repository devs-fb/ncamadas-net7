using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModeloSimples.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VersaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoa",
                columns: table => new
                {
                    PessoaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Criado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modificado = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Removido = table.Column<bool>(type: "bit", nullable: false),
                    Bloquado = table.Column<bool>(type: "bit", nullable: false),
                    Versao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa", x => x.PessoaId);
                });

            migrationBuilder.CreateTable(
                name: "PessoaFisica",
                columns: table => new
                {
                    PessoaFisicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaFisica", x => x.PessoaFisicaId);
                    table.ForeignKey(
                        name: "FK_PessoaFisica_Pessoa_PessoaFisicaId",
                        column: x => x.PessoaFisicaId,
                        principalTable: "Pessoa",
                        principalColumn: "PessoaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PessoaJuridica",
                columns: table => new
                {
                    PessoaJuridicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RazaoSocial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomeFantasia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CNAE = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaJuridica", x => x.PessoaJuridicaId);
                    table.ForeignKey(
                        name: "FK_PessoaJuridica_Pessoa_PessoaJuridicaId",
                        column: x => x.PessoaJuridicaId,
                        principalTable: "Pessoa",
                        principalColumn: "PessoaId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PessoaFisica");

            migrationBuilder.DropTable(
                name: "PessoaJuridica");

            migrationBuilder.DropTable(
                name: "Pessoa");
        }
    }
}
