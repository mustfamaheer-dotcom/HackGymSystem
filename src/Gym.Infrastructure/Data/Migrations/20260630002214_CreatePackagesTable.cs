using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatePackagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("06b4e2a3-3e64-43c7-89e2-260601a839cf"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("107f6514-cd37-4fb8-b968-b03a4ac22252"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("14aca163-64ae-4262-b1d3-dec8f8cf22f9"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("201a833a-0392-4cfb-a183-bb194765246b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("29a4c7a0-3ada-4bb3-ad8b-e1991249ffc1"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("32c7f66d-e01b-4fe0-8278-c7a1af30758a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("42192226-ece7-4a83-94be-889b1e5790a2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6d0bf28b-d1ba-4cd3-8dbc-e624696bb6a0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6eec2950-8e6b-40be-b8cf-efac072b2d95"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("73478d27-963d-4d94-b1e1-0df19d6d4363"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("9420a690-1fd3-4cc2-abc5-7ef8194b7221"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b68512da-be45-4737-9d43-6adb35fddd08"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b9b30068-bd6d-49a0-9b4c-3b05d626304e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c5bc220a-727f-4e20-b228-49d4257ef293"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c7607ce7-ed6f-43d3-be84-a111800c729e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("cacbaf21-ef69-4fdd-9f27-33d30db07844"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("cc4fe0d6-7401-4ada-9983-87d14d0e7ca7"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce6b1312-f7eb-470e-831a-acab73c726c9"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d4c5f256-3f32-4952-8b5b-24faef7eb9d2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d53fe3d2-c395-44c4-bfad-4f70cdc62a25"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e34bdae1-8312-4e6f-8adc-c9f9147e2932"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e997dc01-ab9b-4ec8-90f0-0b18e36d7b03"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f1cb26e3-44fd-4ee0-bd5f-90d3f2efef9b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f35e202a-ae28-451d-8b52-aa2641e79bda"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fcc22e3f-56dd-41b8-b10f-c0ab7334ce3f"));

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    FreePeriodMonths = table.Column<int>(type: "int", nullable: true),
                    MaxFreezeDays = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2221));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2222));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(1177));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2196));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2191));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2182));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 600, DateTimeKind.Utc).AddTicks(2220));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("04e8f242-b73d-45d3-9e78-24b5f992017c"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("10020613-5acb-4350-b7b0-c377d90e67a1"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("10fec474-03c8-416c-893f-1b9e2fb57135"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("19b5c70f-18ca-423f-8fe0-dc42d79f980b"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("2a635dc7-9f45-4eea-ae45-4096a84085b4"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("2f231b37-db29-457a-9a2b-96f9f8822138"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("41638a05-8850-4d60-a5a5-12939b8ef955"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("4b75b558-cf77-43a6-ae12-9238b8417e18"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("4fdb6a94-c071-46a2-8f17-c4dbce3e302b"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("50ae5e39-dadf-459b-aa62-9729af97233e"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("637da492-5705-4f90-91b6-e672baad10b7"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6549e7e0-65dd-4679-81b1-c98280701037"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("830182ee-abd2-4b42-a9f5-5c8d496e6174"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("8873697e-47d2-4f37-8783-82d85b84e991"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("8e7a9823-e8fe-4986-91d9-5496381d8bcc"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("97a57c51-ca8f-4944-914f-9f875f881ee1"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("ab78c11f-f655-4898-ba30-27c927c25440"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("acf601f3-1da5-423b-8ff0-85d5f43ea386"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("b00855ba-6df2-4689-9c59-9d899e43b72a"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("d320fed0-559b-47cd-9eb8-88bc410b1ae2"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d6033d44-5645-42cb-90a3-8639e470ec17"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("d8e14f1c-fc46-47b7-82fd-d4956c0d7e75"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e2314de8-dafd-4fa9-96cb-89358a55723b"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("f980d7e9-1062-40a3-bb0c-14610f7bc282"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("fe9b534c-25b9-4f47-ad2f-06d103554579"), new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(10), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 190, DateTimeKind.Utc).AddTicks(1234));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 190, DateTimeKind.Utc).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 190, DateTimeKind.Utc).AddTicks(2337));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 0, 22, 13, 601, DateTimeKind.Utc).AddTicks(7501));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$rCj35iuQWzh8Xw98GbFx5eW7F9XNxE.IK2fN7Mrh.KoPbsK91ePMS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("04e8f242-b73d-45d3-9e78-24b5f992017c"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("10020613-5acb-4350-b7b0-c377d90e67a1"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("10fec474-03c8-416c-893f-1b9e2fb57135"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("19b5c70f-18ca-423f-8fe0-dc42d79f980b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2a635dc7-9f45-4eea-ae45-4096a84085b4"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2f231b37-db29-457a-9a2b-96f9f8822138"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("41638a05-8850-4d60-a5a5-12939b8ef955"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4b75b558-cf77-43a6-ae12-9238b8417e18"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4fdb6a94-c071-46a2-8f17-c4dbce3e302b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("50ae5e39-dadf-459b-aa62-9729af97233e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("637da492-5705-4f90-91b6-e672baad10b7"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6549e7e0-65dd-4679-81b1-c98280701037"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("830182ee-abd2-4b42-a9f5-5c8d496e6174"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8873697e-47d2-4f37-8783-82d85b84e991"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8e7a9823-e8fe-4986-91d9-5496381d8bcc"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("97a57c51-ca8f-4944-914f-9f875f881ee1"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ab78c11f-f655-4898-ba30-27c927c25440"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("acf601f3-1da5-423b-8ff0-85d5f43ea386"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b00855ba-6df2-4689-9c59-9d899e43b72a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d320fed0-559b-47cd-9eb8-88bc410b1ae2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d6033d44-5645-42cb-90a3-8639e470ec17"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d8e14f1c-fc46-47b7-82fd-d4956c0d7e75"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2314de8-dafd-4fa9-96cb-89358a55723b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f980d7e9-1062-40a3-bb0c-14610f7bc282"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fe9b534c-25b9-4f47-ad2f-06d103554579"));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1610));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1580));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1632));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1611));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1587));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1618));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1592));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1599));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1644));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1620));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1601));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 993, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1622));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1609));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1571));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 994, DateTimeKind.Utc).AddTicks(1626));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("06b4e2a3-3e64-43c7-89e2-260601a839cf"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("107f6514-cd37-4fb8-b968-b03a4ac22252"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("14aca163-64ae-4262-b1d3-dec8f8cf22f9"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("201a833a-0392-4cfb-a183-bb194765246b"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("29a4c7a0-3ada-4bb3-ad8b-e1991249ffc1"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("32c7f66d-e01b-4fe0-8278-c7a1af30758a"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("42192226-ece7-4a83-94be-889b1e5790a2"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("6d0bf28b-d1ba-4cd3-8dbc-e624696bb6a0"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("6eec2950-8e6b-40be-b8cf-efac072b2d95"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("73478d27-963d-4d94-b1e1-0df19d6d4363"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("9420a690-1fd3-4cc2-abc5-7ef8194b7221"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b68512da-be45-4737-9d43-6adb35fddd08"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b9b30068-bd6d-49a0-9b4c-3b05d626304e"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c5bc220a-727f-4e20-b228-49d4257ef293"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c7607ce7-ed6f-43d3-be84-a111800c729e"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("cacbaf21-ef69-4fdd-9f27-33d30db07844"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("cc4fe0d6-7401-4ada-9983-87d14d0e7ca7"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ce6b1312-f7eb-470e-831a-acab73c726c9"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d4c5f256-3f32-4952-8b5b-24faef7eb9d2"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("d53fe3d2-c395-44c4-bfad-4f70cdc62a25"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e34bdae1-8312-4e6f-8adc-c9f9147e2932"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e997dc01-ab9b-4ec8-90f0-0b18e36d7b03"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("f1cb26e3-44fd-4ee0-bd5f-90d3f2efef9b"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("f35e202a-ae28-451d-8b52-aa2641e79bda"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("fcc22e3f-56dd-41b8-b10f-c0ab7334ce3f"), new DateTime(2026, 6, 29, 23, 32, 21, 995, DateTimeKind.Utc).AddTicks(4574), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 544, DateTimeKind.Utc).AddTicks(9533));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 545, DateTimeKind.Utc).AddTicks(596));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 545, DateTimeKind.Utc).AddTicks(597));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 23, 32, 21, 996, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$UC/OIv18WQtlCO8Jnpb4K.u/RgKMqULuegoMYz9pyZo80IYY0rqly");
        }
    }
}
