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
    [Migration("20190324214534_CompanyAndProjectUpdate")]
    partial class CompanyAndProjectUpdate
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

                    b.Property<string>("CompanyId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("ProjectId");

                    b.Property<int>("Role");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("TimeSheetAPI.Models.Activity", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Project", "Project")
                        .WithMany("Activitys")
                        .HasForeignKey("ProjectId");
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

            modelBuilder.Entity("TimeSheetAPI.Models.User", b =>
                {
                    b.HasOne("TimeSheetAPI.Models.Project")
                        .WithMany("UsersOnTheProject")
                        .HasForeignKey("ProjectId");
                });
#pragma warning restore 612, 618
        }
    }
}
