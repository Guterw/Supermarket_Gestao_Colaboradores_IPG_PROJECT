using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hierarquias.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoFuncionarios2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Funcionarios_FuncionariosId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_FuncionariosId",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "FuncionariosId",
                table: "Funcionarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FuncionariosId",
                table: "Funcionarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_FuncionariosId",
                table: "Funcionarios",
                column: "FuncionariosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Funcionarios_FuncionariosId",
                table: "Funcionarios",
                column: "FuncionariosId",
                principalTable: "Funcionarios",
                principalColumn: "Id");
        }
    }
}
