using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon_segunda_chamada.Migrations
{
    /// <inheritdoc />
    public partial class motivorecusa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MotivoRecusa",
                table: "RequerimentosSegundaChamada",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MotivoRecusa",
                table: "RequerimentosSegundaChamada");
        }
    }
}
