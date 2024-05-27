using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ATMS.Web.BankMvc.Migrations
{
    /// <inheritdoc />
    public partial class Removed_SessionInterval_Field_In_UserSession_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionInterval",
                table: "UserSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionInterval",
                table: "UserSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
