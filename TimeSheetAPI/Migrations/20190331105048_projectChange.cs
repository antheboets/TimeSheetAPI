using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeSheetAPI.Migrations
{
    public partial class projectChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OverUren",
                table: "Project",
                newName: "Overtime");

            migrationBuilder.RenameColumn(
                name: "Billabel",
                table: "Project",
                newName: "Billable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Overtime",
                table: "Project",
                newName: "OverUren");

            migrationBuilder.RenameColumn(
                name: "Billable",
                table: "Project",
                newName: "Billabel");
        }
    }
}
