using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePackagesModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5030));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5024));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5028));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5028));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(4105));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5033));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5022));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 888, DateTimeKind.Utc).AddTicks(5034));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("38e68501-6aa4-2723-251e-83ab85145eda"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("411c0941-f12c-e117-e542-7561547fae68"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("618876f6-daa1-fc83-c047-480bf88c794e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 889, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 457, DateTimeKind.Utc).AddTicks(945));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 457, DateTimeKind.Utc).AddTicks(1891));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 457, DateTimeKind.Utc).AddTicks(1893));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 22, 5, 20, 890, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$rSsZIqkXcxzB1goFPOdWjOZvy7WsCLM4vJpVoTNXBtfNTk//jmB4G");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    FreePeriodMonths = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MaxFreezeDays = table.Column<int>(type: "int", nullable: true),
                    PackageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8290));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8285));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8313));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8291));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8286));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8292));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8287));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8288));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8313));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8293));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8289));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8294));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8289));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8283));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 919, DateTimeKind.Utc).AddTicks(8312));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("38e68501-6aa4-2723-251e-83ab85145eda"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("411c0941-f12c-e117-e542-7561547fae68"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("618876f6-daa1-fc83-c047-480bf88c794e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 920, DateTimeKind.Utc).AddTicks(5851));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 661, DateTimeKind.Utc).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 661, DateTimeKind.Utc).AddTicks(9990));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 661, DateTimeKind.Utc).AddTicks(9991));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 0, 45, 56, 922, DateTimeKind.Utc).AddTicks(2476));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$LtsHBGnYCL6feoD3R4m9bewEMSN6eEBLi5kOdnm4uJ.YGa8Gnn15a");
        }
    }
}
