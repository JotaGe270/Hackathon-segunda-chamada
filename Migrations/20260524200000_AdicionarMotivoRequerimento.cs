using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hackathon_segunda_chamada.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarMotivoRequerimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Motivo",
                table: "RequerimentosSegundaChamada",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Motivo",
                table: "RequerimentosSegundaChamada");
        }
    }
}
