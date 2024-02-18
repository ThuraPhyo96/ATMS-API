using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ATMS.Web.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerGuid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    NRIC = table.Column<string>(maxLength: 50, nullable: true),
                    FatherName = table.Column<string>(maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    BankAccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankAccountGuid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<int>(nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 20, nullable: true),
                    AccountType = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.BankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BalanceHistories",
                columns: table => new
                {
                    BalanceHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BalanceHistoryGuid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<int>(nullable: false),
                    BankAccountId = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HistoryType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceHistories", x => x.BalanceHistoryId);
                    table.ForeignKey(
                        name: "FK_BalanceHistories_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BalanceHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BankCards",
                columns: table => new
                {
                    BankCardId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankCardGuid = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    CustomerId = table.Column<int>(nullable: false),
                    BankAccountId = table.Column<int>(nullable: false),
                    BankCardNumber = table.Column<string>(maxLength: 20, nullable: true),
                    PIN = table.Column<string>(nullable: true),
                    ValidDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankCards", x => x.BankCardId);
                    table.ForeignKey(
                        name: "FK_BankCards_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAccountId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankCards_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceHistories_BalanceHistoryGuid",
                table: "BalanceHistories",
                column: "BalanceHistoryGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalanceHistories_BankAccountId",
                table: "BalanceHistories",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceHistories_CustomerId",
                table: "BalanceHistories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankAccountGuid",
                table: "BankAccounts",
                column: "BankAccountGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CustomerId",
                table: "BankAccounts",
                column: "CustomerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankCards_BankAccountId",
                table: "BankCards",
                column: "BankAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankCards_BankCardGuid",
                table: "BankCards",
                column: "BankCardGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankCards_CustomerId",
                table: "BankCards",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerGuid",
                table: "Customers",
                column: "CustomerGuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalanceHistories");

            migrationBuilder.DropTable(
                name: "BankCards");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
