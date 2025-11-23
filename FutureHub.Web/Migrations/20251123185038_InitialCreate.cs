using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FutureHub.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_FH_AREAS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FH_AREAS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_FH_USUARIOS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    NOME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    SENHA_HASH = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    ROLE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false, defaultValue: "ROLE_USER"),
                    AREA_INTERESSE_ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: true),
                    PONTOS = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FH_USUARIOS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_USUARIO_AREA",
                        column: x => x.AREA_INTERESSE_ID,
                        principalTable: "T_FH_AREAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "T_FH_IDEIAS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    TITULO = table.Column<string>(type: "NVARCHAR2(160)", maxLength: 160, nullable: false),
                    DESCRICAO = table.Column<string>(type: "NVARCHAR2(2000)", maxLength: 2000, nullable: false),
                    AUTOR_ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    MISSAO_ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: true),
                    MEDIA_NOTAS = table.Column<double>(type: "BINARY_DOUBLE", precision: 3, scale: 2, nullable: false, defaultValue: 0.0),
                    TOTAL_AVALIACOES = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValue: 0),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FH_IDEIAS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_IDEIA_AUTOR",
                        column: x => x.AUTOR_ID,
                        principalTable: "T_FH_USUARIOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_FH_RANKINGS",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    USUARIO_ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    PONTUACAO_TOTAL = table.Column<int>(type: "NUMBER(10)", nullable: false, defaultValue: 0),
                    PERIODO = table.Column<string>(type: "NVARCHAR2(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FH_RANKINGS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RANKING_USUARIO",
                        column: x => x.USUARIO_ID,
                        principalTable: "T_FH_USUARIOS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_FH_AVALIACOES",
                columns: table => new
                {
                    ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    IDEIA_ID = table.Column<string>(type: "NVARCHAR2(36)", maxLength: 36, nullable: false),
                    NOTA = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DATA_AVALIACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_FH_AVALIACOES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AVALIACAO_IDEIA",
                        column: x => x.IDEIA_ID,
                        principalTable: "T_FH_IDEIAS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UK_AREA_NOME",
                table: "T_FH_AREAS",
                column: "NOME",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_AVALIACAO_DATA",
                table: "T_FH_AVALIACOES",
                column: "DATA_AVALIACAO");

            migrationBuilder.CreateIndex(
                name: "IDX_AVALIACAO_IDEIA",
                table: "T_FH_AVALIACOES",
                column: "IDEIA_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_IDEIA_AUTOR",
                table: "T_FH_IDEIAS",
                column: "AUTOR_ID");

            migrationBuilder.CreateIndex(
                name: "IDX_IDEIA_CREATED",
                table: "T_FH_IDEIAS",
                column: "CREATED_AT");

            migrationBuilder.CreateIndex(
                name: "IDX_IDEIA_MEDIA",
                table: "T_FH_IDEIAS",
                column: "MEDIA_NOTAS");

            migrationBuilder.CreateIndex(
                name: "IDX_RANKING_PERIODO",
                table: "T_FH_RANKINGS",
                column: "PERIODO");

            migrationBuilder.CreateIndex(
                name: "IDX_RANKING_PONTUACAO",
                table: "T_FH_RANKINGS",
                column: "PONTUACAO_TOTAL");

            migrationBuilder.CreateIndex(
                name: "IDX_RANKING_USUARIO",
                table: "T_FH_RANKINGS",
                column: "USUARIO_ID");

            migrationBuilder.CreateIndex(
                name: "UK_RANKING_USUARIO_PERIODO",
                table: "T_FH_RANKINGS",
                columns: new[] { "USUARIO_ID", "PERIODO" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IDX_USUARIO_AREA",
                table: "T_FH_USUARIOS",
                column: "AREA_INTERESSE_ID");

            migrationBuilder.CreateIndex(
                name: "UK_USUARIO_EMAIL",
                table: "T_FH_USUARIOS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_FH_AVALIACOES");

            migrationBuilder.DropTable(
                name: "T_FH_RANKINGS");

            migrationBuilder.DropTable(
                name: "T_FH_IDEIAS");

            migrationBuilder.DropTable(
                name: "T_FH_USUARIOS");

            migrationBuilder.DropTable(
                name: "T_FH_AREAS");
        }
    }
}
