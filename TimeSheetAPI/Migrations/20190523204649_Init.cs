using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeSheetAPI.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkDay",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Stop = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CompanyId = table.Column<string>(nullable: true),
                    InProgress = table.Column<bool>(nullable: false),
                    Overtime = table.Column<bool>(nullable: false),
                    Billable = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
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
                        name: "FK_DefaultWorkweek_WorkDay_FridayId",
                        column: x => x.FridayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_WorkDay_MondayId",
                        column: x => x.MondayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_WorkDay_SaturdayId",
                        column: x => x.SaturdayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_WorkDay_SundayId",
                        column: x => x.SundayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_WorkDay_ThursdayId",
                        column: x => x.ThursdayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_WorkDay_TuesdayId",
                        column: x => x.TuesdayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DefaultWorkweek_WorkDay_WednesdayId",
                        column: x => x.WednesdayId,
                        principalTable: "WorkDay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    DefaultWorkweekId = table.Column<string>(nullable: true),
                    ChangeHistory = table.Column<bool>(nullable: false),
                    HourlyRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_DefaultWorkweek_DefaultWorkweekId",
                        column: x => x.DefaultWorkweekId,
                        principalTable: "DefaultWorkweek",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Stop = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ProjectId = table.Column<string>(nullable: true),
                    ActivityId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Log_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Log_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUser",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ProjectId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUser", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ProjectUser_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkDayException",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Stop = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDayException", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkDayException_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkMonth",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Accepted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkMonth", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkMonth_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ProjectId",
                table: "Activity",
                column: "ProjectId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Log_ActivityId",
                table: "Log",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_ProjectId",
                table: "Log",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_UserId",
                table: "Log",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_CompanyId",
                table: "Project",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUser_UserId",
                table: "ProjectUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_DefaultWorkweekId",
                table: "User",
                column: "DefaultWorkweekId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkDayException_UserId",
                table: "WorkDayException",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkMonth_UserId",
                table: "WorkMonth",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "ProjectUser");

            migrationBuilder.DropTable(
                name: "WorkDayException");

            migrationBuilder.DropTable(
                name: "WorkMonth");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "DefaultWorkweek");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "WorkDay");
        }
    }
}
