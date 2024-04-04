using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMS.Web.ATMLocationAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_Inititial_Setup_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankNames",
                columns: table => new
                {
                    BankNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankNames", x => x.BankNameId);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    RegionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.RegionId);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionId);
                    table.ForeignKey(
                        name: "FK_Divisions_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Townships",
                columns: table => new
                {
                    TownshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Townships", x => x.TownshipId);
                    table.ForeignKey(
                        name: "FK_Townships_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Townships_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BankBranchNames",
                columns: table => new
                {
                    BankBranchNameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankNameId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    TownshipId = table.Column<int>(type: "int", nullable: false),
                    OpeningHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosedHour = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankBranchNames", x => x.BankBranchNameId);
                    table.ForeignKey(
                        name: "FK_BankBranchNames_BankNames_BankNameId",
                        column: x => x.BankNameId,
                        principalTable: "BankNames",
                        principalColumn: "BankNameId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankBranchNames_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankBranchNames_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankBranchNames_Townships_TownshipId",
                        column: x => x.TownshipId,
                        principalTable: "Townships",
                        principalColumn: "TownshipId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ATMLocations",
                columns: table => new
                {
                    ATMLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankNameId = table.Column<int>(type: "int", nullable: false),
                    BankBranchNameId = table.Column<int>(type: "int", nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    TownshipId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ATMLocations", x => x.ATMLocationId);
                    table.ForeignKey(
                        name: "FK_ATMLocations_BankBranchNames_BankBranchNameId",
                        column: x => x.BankBranchNameId,
                        principalTable: "BankBranchNames",
                        principalColumn: "BankBranchNameId");
                    table.ForeignKey(
                        name: "FK_ATMLocations_BankNames_BankNameId",
                        column: x => x.BankNameId,
                        principalTable: "BankNames",
                        principalColumn: "BankNameId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ATMLocations_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ATMLocations_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ATMLocations_Townships_TownshipId",
                        column: x => x.TownshipId,
                        principalTable: "Townships",
                        principalColumn: "TownshipId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ATMLocations_BankBranchNameId",
                table: "ATMLocations",
                column: "BankBranchNameId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMLocations_BankNameId",
                table: "ATMLocations",
                column: "BankNameId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMLocations_DivisionId",
                table: "ATMLocations",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMLocations_RegionId",
                table: "ATMLocations",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ATMLocations_TownshipId",
                table: "ATMLocations",
                column: "TownshipId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranchNames_BankNameId",
                table: "BankBranchNames",
                column: "BankNameId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranchNames_DivisionId",
                table: "BankBranchNames",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranchNames_RegionId",
                table: "BankBranchNames",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranchNames_TownshipId",
                table: "BankBranchNames",
                column: "TownshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_RegionId",
                table: "Divisions",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Townships_DivisionId",
                table: "Townships",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Townships_RegionId",
                table: "Townships",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ATMLocations");

            migrationBuilder.DropTable(
                name: "BankBranchNames");

            migrationBuilder.DropTable(
                name: "BankNames");

            migrationBuilder.DropTable(
                name: "Townships");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
