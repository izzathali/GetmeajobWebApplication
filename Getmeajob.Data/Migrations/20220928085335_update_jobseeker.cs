using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Getmeajob.Data.Migrations
{
    public partial class update_jobseeker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobSeekerName",
                table: "JobSeekers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobSeekerName",
                table: "JobSeekers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
