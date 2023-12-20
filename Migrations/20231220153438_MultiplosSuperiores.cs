using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hierarquias.Migrations
{
    /// <inheritdoc />
    public partial class MultiplosSuperiores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funcionarios_Funcionarios_SuperiorId",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_SuperiorId",
                table: "Funcionarios");

            migrationBuilder.DropColumn(
                name: "SuperiorId",
                table: "Funcionarios");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FuncionariosSuperiores",
                columns: table => new
                {
                    SubordinadoId = table.Column<int>(type: "int", nullable: false),
                    SuperiorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuncionariosSuperiores", x => new { x.SubordinadoId, x.SuperiorId });
                    table.ForeignKey(
                        name: "FK_FuncionariosSuperiores_Funcionarios_SubordinadoId",
                        column: x => x.SubordinadoId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FuncionariosSuperiores_Funcionarios_SuperiorId",
                        column: x => x.SuperiorId,
                        principalTable: "Funcionarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuncionariosSuperiores_SuperiorId",
                table: "FuncionariosSuperiores",
                column: "SuperiorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuncionariosSuperiores");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Funcionarios",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "SuperiorId",
                table: "Funcionarios",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_SuperiorId",
                table: "Funcionarios",
                column: "SuperiorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Funcionarios_Funcionarios_SuperiorId",
                table: "Funcionarios",
                column: "SuperiorId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
