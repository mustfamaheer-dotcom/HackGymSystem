using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataMigrations
{
    /// <inheritdoc />
    public partial class AddDailySessionsPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d1000017-0000-0000-0000-000000000001"), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "View daily sessions", "Daily Sessions", "DailySessions.View", null },
                    { new Guid("d1000017-0000-0000-0000-000000000002"), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Create daily sessions", "Daily Sessions", "DailySessions.Create", null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1d5258ab-e319-427b-0bc8-58a1d66d17f2"), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000017-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("81874dec-6fb7-347a-9123-905f7c6889c7"), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000017-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("c5225560-c5ba-9944-60af-626bd395e6b5"), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000017-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("f101e791-2da5-b3d5-471e-07ac41fa30bc"), new DateTime(2026, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("d1000017-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1d5258ab-e319-427b-0bc8-58a1d66d17f2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("81874dec-6fb7-347a-9123-905f7c6889c7"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c5225560-c5ba-9944-60af-626bd395e6b5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f101e791-2da5-b3d5-471e-07ac41fa30bc"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000017-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000017-0000-0000-0000-000000000002"));
        }
    }
}
