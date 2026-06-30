using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailDobGenderNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1900a968-b8f6-425d-89a3-09a344f32341"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("23681755-d602-4f32-8224-4a35999a165a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("23b4e224-9e35-4e8c-ae2a-20e550f9f839"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2c2fa57d-3700-46ef-baf5-a83b9deb47ce"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("320fd0c2-d4fb-40de-b0d1-4b3ae4051ed6"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("355745ae-d2cf-411b-9b8b-63dec21c8af2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("41c5b2c9-2871-4cc4-af57-c5b2991598f4"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4a29139f-b306-47fb-8513-a2f1193ed0fd"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4a366db1-b3f1-43ed-898f-2ea474eaf709"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("59c0d8cb-b976-487f-a098-1330f6e9082a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7487240c-b9bb-491a-bc54-69fad79a1573"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("77864273-21ab-4fc7-8a3d-ef3afc51f912"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("780f2da0-31ef-4559-9ffe-1262ed47389d"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8d84b051-ef2a-416a-828c-68bbf7cf5b24"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("9dccbc0a-9798-4823-9626-88b35df580e6"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a90c1bd8-05df-4c01-b0d2-014b3a11d8fd"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ac7822ca-3bbc-4669-9b6b-07e6f49d45fc"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("aed39055-9d59-4d6b-9811-6956e05bff29"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b5f48274-e6d5-4d17-bd4b-aba6a020d9b4"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c72c22d3-d59c-4d46-9a1e-d12267783235"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce9e7b1f-35c6-4c9e-b26e-c7e635ee0083"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("cfd7c3f6-1bc8-4e62-97fd-b2e60c64aa3d"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ef2c129f-b76a-4d20-8b4b-462f4deabbbd"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f3d21642-78ec-4ee7-9b2c-4b9893a99ac0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f847c126-7fc4-4a31-8198-a7c02bef0c7f"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Members",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Members",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Members",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Members");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7083));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7061));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7084));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7085));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7077));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7079));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7198));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7087));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7080));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7088));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7082));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1900a968-b8f6-425d-89a3-09a344f32341"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("23681755-d602-4f32-8224-4a35999a165a"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("23b4e224-9e35-4e8c-ae2a-20e550f9f839"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("2c2fa57d-3700-46ef-baf5-a83b9deb47ce"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("320fd0c2-d4fb-40de-b0d1-4b3ae4051ed6"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("355745ae-d2cf-411b-9b8b-63dec21c8af2"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("41c5b2c9-2871-4cc4-af57-c5b2991598f4"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("4a29139f-b306-47fb-8513-a2f1193ed0fd"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("4a366db1-b3f1-43ed-898f-2ea474eaf709"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("59c0d8cb-b976-487f-a098-1330f6e9082a"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7487240c-b9bb-491a-bc54-69fad79a1573"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("77864273-21ab-4fc7-8a3d-ef3afc51f912"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("780f2da0-31ef-4559-9ffe-1262ed47389d"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("8d84b051-ef2a-416a-828c-68bbf7cf5b24"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("9dccbc0a-9798-4823-9626-88b35df580e6"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("a90c1bd8-05df-4c01-b0d2-014b3a11d8fd"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("ac7822ca-3bbc-4669-9b6b-07e6f49d45fc"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("aed39055-9d59-4d6b-9811-6956e05bff29"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b5f48274-e6d5-4d17-bd4b-aba6a020d9b4"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c72c22d3-d59c-4d46-9a1e-d12267783235"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("ce9e7b1f-35c6-4c9e-b26e-c7e635ee0083"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("cfd7c3f6-1bc8-4e62-97fd-b2e60c64aa3d"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ef2c129f-b76a-4d20-8b4b-462f4deabbbd"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("f3d21642-78ec-4ee7-9b2c-4b9893a99ac0"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("f847c126-7fc4-4a31-8198-a7c02bef0c7f"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 186, DateTimeKind.Utc).AddTicks(5414));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 186, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 186, DateTimeKind.Utc).AddTicks(6421));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 29, 22, 50, 43, 473, DateTimeKind.Utc).AddTicks(3349));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$Skgb0mL5DIF3N88WtHaCpeyyeK2k7TRfKtr2uc4qF7l5QfvpuvJ6G");
        }
    }
}
