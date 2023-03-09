using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCore.Migrations
{
    public partial class add_admin_account : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Address", "Email", "FirstName", "IsDeleted", "IsLockout", "LastName", "Password", "Phone", "RoleId" },
                values: new object[] { 6, "abc", "admin1@email", "abc", false, false, "abc", "123", "0908123456", 1 });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2023, 3, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 3, 19, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 3, 9, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "EndDate", "StartDate" },
                values: new object[] { new DateTime(2023, 2, 27, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 3, 9, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2023, 2, 27, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
