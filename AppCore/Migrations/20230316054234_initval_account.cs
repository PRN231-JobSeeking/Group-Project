using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCore.Migrations
{
    public partial class initval_account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IsDeleted", "IsLockout", "LastName", "Password", "Phone", "RoleId" },
                values: new object[] { 7, "abc", "applicant02@email", "abc", false, false, "abc", "123", "0908123456", 4 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
