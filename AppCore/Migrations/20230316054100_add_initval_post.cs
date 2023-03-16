using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppCore.Migrations
{
    public partial class add_initval_post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "Amount", "CategoryId", "CreateDate", "Description", "EndDate", "IsDeleted", "LevelId", "LocationId", "StartDate", "Status", "Title" },
                values: new object[] { 2, 1, 2, new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Local), "abcdef", new DateTime(2023, 3, 26, 0, 0, 0, 0, DateTimeKind.Local), false, 1, 1, new DateTime(2023, 3, 16, 0, 0, 0, 0, DateTimeKind.Local), true, "Frontend hiring" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
