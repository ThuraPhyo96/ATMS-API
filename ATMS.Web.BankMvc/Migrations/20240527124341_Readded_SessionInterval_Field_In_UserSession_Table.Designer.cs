﻿// <auto-generated />
using System;
using ATMS.Web.BankMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ATMS.Web.BankMvc.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20240527124341_Readded_SessionInterval_Field_In_UserSession_Table")]
    partial class Readded_SessionInterval_Field_In_UserSession_Table
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ATMS.Web.Dto.Models.ATMLocation", b =>
                {
                    b.Property<int>("ATMLocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ATMLocationId"));

                    b.Property<string>("Address")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("BankBranchNameId")
                        .HasColumnType("int");

                    b.Property<int>("BankNameId")
                        .HasColumnType("int");

                    b.Property<int>("DivisionId")
                        .HasColumnType("int");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("TownshipId")
                        .HasColumnType("int");

                    b.HasKey("ATMLocationId");

                    b.HasIndex("BankBranchNameId");

                    b.HasIndex("BankNameId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("RegionId");

                    b.HasIndex("TownshipId");

                    b.ToTable("ATMLocations");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.BankBranchName", b =>
                {
                    b.Property<int>("BankBranchNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankBranchNameId"));

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("BankNameId")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("ClosedHour")
                        .HasColumnType("time");

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<int>("DivisionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<TimeSpan>("OpeningHour")
                        .HasColumnType("time");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("TelephoneNumber")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("TownshipId")
                        .HasColumnType("int");

                    b.HasKey("BankBranchNameId");

                    b.HasIndex("BankNameId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("RegionId");

                    b.HasIndex("TownshipId");

                    b.ToTable("BankBranchNames");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.BankName", b =>
                {
                    b.Property<int>("BankNameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankNameId"));

                    b.Property<string>("Address")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Code")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TelephoneNumber")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("BankNameId");

                    b.ToTable("BankNames");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Division", b =>
                {
                    b.Property<int>("DivisionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DivisionId"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.HasKey("DivisionId");

                    b.HasIndex("RegionId");

                    b.ToTable("Divisions");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Region", b =>
                {
                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegionId"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.HasKey("RegionId");

                    b.ToTable("Regions");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Township", b =>
                {
                    b.Property<int>("TownshipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TownshipId"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("DivisionId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.HasKey("TownshipId");

                    b.HasIndex("DivisionId");

                    b.HasIndex("RegionId");

                    b.ToTable("Townships");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.UserSession", b =>
                {
                    b.Property<Guid>("UserSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SessionInterval")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserSessionId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.ATMLocation", b =>
                {
                    b.HasOne("ATMS.Web.Dto.Models.BankBranchName", "BankBranchName")
                        .WithMany()
                        .HasForeignKey("BankBranchNameId");

                    b.HasOne("ATMS.Web.Dto.Models.BankName", "BankName")
                        .WithMany()
                        .HasForeignKey("BankNameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Division", "Division")
                        .WithMany()
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Township", "Township")
                        .WithMany()
                        .HasForeignKey("TownshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankBranchName");

                    b.Navigation("BankName");

                    b.Navigation("Division");

                    b.Navigation("Region");

                    b.Navigation("Township");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.BankBranchName", b =>
                {
                    b.HasOne("ATMS.Web.Dto.Models.BankName", "BankName")
                        .WithMany("BankBranchNames")
                        .HasForeignKey("BankNameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Division", "Division")
                        .WithMany()
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Township", "Township")
                        .WithMany()
                        .HasForeignKey("TownshipId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankName");

                    b.Navigation("Division");

                    b.Navigation("Region");

                    b.Navigation("Township");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Division", b =>
                {
                    b.HasOne("ATMS.Web.Dto.Models.Region", "Region")
                        .WithMany("Divisions")
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Township", b =>
                {
                    b.HasOne("ATMS.Web.Dto.Models.Division", "Division")
                        .WithMany("Townships")
                        .HasForeignKey("DivisionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ATMS.Web.Dto.Models.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Division");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.UserSession", b =>
                {
                    b.HasOne("ATMS.Web.Dto.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.BankName", b =>
                {
                    b.Navigation("BankBranchNames");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Division", b =>
                {
                    b.Navigation("Townships");
                });

            modelBuilder.Entity("ATMS.Web.Dto.Models.Region", b =>
                {
                    b.Navigation("Divisions");
                });
#pragma warning restore 612, 618
        }
    }
}
