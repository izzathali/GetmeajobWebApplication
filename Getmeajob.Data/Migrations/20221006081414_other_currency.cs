using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Getmeajob.Data.Migrations
{
    public partial class other_currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherCurrency",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherCurrency",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherCurrency",
                table: "Resumes");

            migrationBuilder.DropColumn(
                name: "OtherCurrency",
                table: "Jobs");
        }
    }
}
