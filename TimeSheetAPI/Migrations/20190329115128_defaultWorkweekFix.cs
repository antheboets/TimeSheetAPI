using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeSheetAPI.Migrations
{
    public partial class defaultWorkweekFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DefaultWorkweekId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Billabel",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "OverUren",
                table: "Project",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Day",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Stop = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Day_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DefaultWorkweek",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MondayId = table.Column<string>(nullable: true),
                    TuesdayId = table.Column<string>(nullable: true),
                    WednesdayId = table.Column<string>(nullable: true),
                    ThursdayId = table.Column<string>(nullable: true),
                    FridayId = table.Column<string>(nullable: true),
                    SaturdayId = table.Column<string>(nullable: true),
                    SundayId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultWorkweek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_FridayId",
                        column: x => x.FridayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_MondayId",
                        column: x => x.MondayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_SaturdayId",
                        column: x => x.SaturdayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_SundayId",
                        column: x => x.SundayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_ThursdayId",
                        column: x => x.ThursdayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_TuesdayId",
                        column: x => x.TuesdayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_Day_WednesdayId",
                        column: x => x.WednesdayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_DefaultWorkweekId",
                table: "User",
                column: "DefaultWorkweekId");

            migrationBuilder.CreateIndex(
                name: "IX_Day_UserId",
                table: "Day",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_FridayId",
                table: "DefaultWorkweek",
                column: "FridayId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_MondayId",
                table: "DefaultWorkweek",
                column: "MondayId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_SaturdayId",
                table: "DefaultWorkweek",
                column: "SaturdayId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_SundayId",
                table: "DefaultWorkweek",
                column: "SundayId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_ThursdayId",
                table: "DefaultWorkweek",
                column: "ThursdayId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_TuesdayId",
                table: "DefaultWorkweek",
                column: "TuesdayId");

            migrationBuilder.CreateIndex(
                name: "IX_DefaultWorkweek_WednesdayId",
                table: "DefaultWorkweek",
                column: "WednesdayId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_DefaultWorkweek_DefaultWorkweekId",
                table: "User",
                column: "DefaultWorkweekId",
                principalTable: "DefaultWorkweek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_DefaultWorkweek_DefaultWorkweekId",
                table: "User");

            migrationBuilder.DropTable(
                name: "DefaultWorkweek");

            migrationBuilder.DropTable(
                name: "Day");

            migrationBuilder.DropIndex(
                name: "IX_User_DefaultWorkweekId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DefaultWorkweekId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Billabel",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "OverUren",
                table: "Project");
        }
    }
}
