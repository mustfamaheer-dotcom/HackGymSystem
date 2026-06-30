using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNationalityNationalIdToRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_NationalId",
                table: "Members");

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

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Members",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4027));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4022));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4046));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4028));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4023));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4029));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4024));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4025));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4046));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4030));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4025));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(2945));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4031));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4026));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 720, DateTimeKind.Utc).AddTicks(4045));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("38e68501-6aa4-2723-251e-83ab85145eda"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("411c0941-f12c-e117-e542-7561547fae68"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("618876f6-daa1-fc83-c047-480bf88c794e"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"), new DateTime(2026, 6, 30, 1, 30, 4, 721, DateTimeKind.Utc).AddTicks(1835), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 494, DateTimeKind.Utc).AddTicks(2667));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 494, DateTimeKind.Utc).AddTicks(3713));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 494, DateTimeKind.Utc).AddTicks(3714));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 1, 30, 4, 723, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$Uuso/x/mhWPcVrNHqin8ru1lvWqNiEXRC9FBNoN4GM9MuJSvZPpUm");

            migrationBuilder.Sql(@"
UPDATE m
SET m.NationalId = LEFT('TEMP-' + REPLACE(CONVERT(NVARCHAR(36), m.Id), '-', ''), 14)
FROM Members m
WHERE m.NationalId = ''
   OR m.NationalId IS NULL
");

            migrationBuilder.CreateIndex(
                name: "IX_Members_NationalId",
                table: "Members",
                column: "NationalId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Members_NationalId",
                table: "Members");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("38e68501-6aa4-2723-251e-83ab85145eda"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("411c0941-f12c-e117-e542-7561547fae68"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("618876f6-daa1-fc83-c047-480bf88c794e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"));

            migrationBuilder.AlterColumn<string>(
                name: "Nationality",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "Members",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(14)",
                oldMaxLength: 14);

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

            migrationBuilder.CreateIndex(
                name: "IX_Members_NationalId",
                table: "Members",
                column: "NationalId",
                unique: true,
                filter: "[NationalId] IS NOT NULL");
        }
    }
}
