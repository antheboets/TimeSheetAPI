﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeSheetAPI.Infrastructure;

namespace TimeSheetAPI.Migrations
{
    [DbContext(typeof(TimeSheetContext))]
    [Migration("20190329115128_defaultWorkweekFix")]
    partial class defaultWorkweekFix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TimeSheetAPI.Models.Activity", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Company", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Company");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Day", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Start");

                    b.Property<DateTime>("Stop");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Day");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.DefaultWorkweek", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FridayId");

                    b.Property<string>("MondayId");

                    b.Property<string>("SaturdayId");

                    b.Property<string>("SundayId");

                    b.Property<string>("ThursdayId");

                    b.Property<string>("TuesdayId");

                    b.Property<string>("WednesdayId");

                    b.HasKey("Id");

                    b.HasIndex("FridayId");

                    b.HasIndex("MondayId");

                    b.HasIndex("SaturdayId");

                    b.HasIndex("SundayId");

                    b.HasIndex("ThursdayId");

                    b.HasIndex("TuesdayId");

                    b.HasIndex("WednesdayId");

                    b.ToTable("DefaultWorkweek");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Log", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityId");

                    b.Property<string>("Description");

                    b.Property<string>("ProjectId");

                    b.Property<DateTime>("Start");

                    b.Property<DateTime>("Stop");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Project", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Billabel");

                    b.Property<string>("CompanyId");

                    b.Property<string>("Name");

                    b.Property<bool>("OverUren");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.ProjectUser", b =>
                {
                    b.Property<string>("ProjectId");

                    b.Property<string>("UserId");

                    b.HasKey("ProjectId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUser");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DefaultWorkweekId");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("ProjectId");

                    b.Property<string>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("DefaultWorkweekId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RoleId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Activity", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Project", "Project")
                        .WithMany("Activitys")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Day", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.User")
                        .WithMany("ExceptionWorkDays")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.DefaultWorkweek", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Day", "Friday")
                        .WithMany()
                        .HasForeignKey("FridayId");

                    b.HasOne("TimeSheetAPI.Models.Day", "Monday")
                        .WithMany()
                        .HasForeignKey("MondayId");

                    b.HasOne("TimeSheetAPI.Models.Day", "Saturday")
                        .WithMany()
                        .HasForeignKey("SaturdayId");

                    b.HasOne("TimeSheetAPI.Models.Day", "Sunday")
                        .WithMany()
                        .HasForeignKey("SundayId");

                    b.HasOne("TimeSheetAPI.Models.Day", "Thursday")
                        .WithMany()
                        .HasForeignKey("ThursdayId");

                    b.HasOne("TimeSheetAPI.Models.Day", "Tuesday")
                        .WithMany()
                        .HasForeignKey("TuesdayId");

                    b.HasOne("TimeSheetAPI.Models.Day", "Wednesday")
                        .WithMany()
                        .HasForeignKey("WednesdayId");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Log", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId");

                    b.HasOne("TimeSheetAPI.Models.Project", "Project")
                        .WithMany("Logs")
                        .HasForeignKey("ProjectId");

                    b.HasOne("TimeSheetAPI.Models.User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Project", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.ProjectUser", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TimeSheetAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TimeSheetAPI.Models.User", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.DefaultWorkweek", "DefaultWorkweek")
                        .WithMany()
                        .HasForeignKey("DefaultWorkweekId");

                    b.HasOne("TimeSheetAPI.Models.Project")
                        .WithMany("UsersOnTheProject")
                        .HasForeignKey("ProjectId");

                    b.HasOne("TimeSheetAPI.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
