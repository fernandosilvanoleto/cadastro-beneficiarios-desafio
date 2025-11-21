using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio_Tecnico.Infraestructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MinhaNovaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Planos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Is_deleted",
                table: "Beneficiarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Planos");

            migrationBuilder.DropColumn(
                name: "Is_deleted",
                table: "Beneficiarios");
        }
    }
}
