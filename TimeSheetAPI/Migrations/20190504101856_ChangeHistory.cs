using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeSheetAPI.Migrations
{
    public partial class ChangeHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ChangeHistory",
                table: "User",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeHistory",
                table: "User");
        }
    }
}
