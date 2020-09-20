using Microsoft.EntityFrameworkCore.Migrations;

namespace JobRegistrationSubmisson.Migrations
{
    public partial class initJobSName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TB_M_JobSeeker",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TB_M_JobSeeker");
        }
    }
}
