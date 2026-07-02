using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWhatsAppTemplateTriggerType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriggerType",
                table: "WhatsAppTemplates",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000001-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000001-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000007-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000007-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000008-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000008-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000009-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000009-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000010-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000010-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000013-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000013-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000014-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000015-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000015-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000016-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000016-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 428, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("005f9b64-4d0a-6c82-947e-e57cfe738452"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("05195fdb-cd30-4561-9168-bbeb6f886a69"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0a025850-9197-728d-2039-14fa13c954ce"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0e32d4f0-e31f-1692-6763-43d06de36c0e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1def5b70-20cb-7e7b-b398-51bae635cb4c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2babdd1d-2af8-85c1-be3d-93660af5d9ca"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2cc74796-4a71-5342-2d08-a37e9154996d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("32c5144e-c485-1183-98b8-403fbb277ebd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("35837f97-2623-ab10-0cbd-370671326f89"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("430af60c-cfb8-d59a-e833-8cd110ff7f06"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("43fe5635-7b4d-ab3d-a51e-3559aced41c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("45a0ab26-2f4b-7f21-a073-171f907faf77"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("477c3b9c-bdb3-a703-0b56-f6516dcfc7e5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4d02d95c-4aab-3930-8c9f-b46c21d1be4e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5334b704-3d25-6b96-517f-6443fb11c414"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("55690f65-12c3-9922-c4b1-47b34caedde2"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("557952bd-50d0-d062-f35e-7fe6224bd482"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("566928b3-00dc-fb90-39b7-af77eb0f35b1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5f9b5de3-e563-a2d9-672a-91816ba23dd6"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("63139b75-fbfc-2bcf-36d2-43bc185b9a05"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6604e362-f7af-bb42-e9e1-448a76c8e188"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6a745601-638c-730b-cda2-d7a67e9f5a16"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6b5ac608-95b1-cd3e-d869-0fdf79a09fd0"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6b6ec0d1-eb14-8d88-e615-78e6fe6177bf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("70ec0ef2-a390-97c2-76ef-1678f6cf8fbf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7297595c-0a29-eb0d-19be-c531ee673c5b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("72bf7e60-3016-f5ff-ec42-ddfc1e27acfc"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7347301a-7584-8075-4aab-788b19078a62"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("78615f0f-b666-f010-e57b-dfb05c66f035"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("78b53d72-0ba7-6249-68a8-bedd284304f4"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7916078f-5d85-86e1-8747-b5dd207f40b5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("82cdc680-60c7-ca13-1fba-0c1725b381fa"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8680abc3-7a3f-fc42-ec4b-282165955f1c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("86f4523e-677d-889c-8e00-7ee5a50d94c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("89d0bc79-33f4-2a4d-f951-da329c328c20"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8ef92960-4f9f-70c2-58b7-a0fc32743d95"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("904ee737-7848-bded-ec47-ed74c6b3b5e5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("9cbf51af-d816-7ca8-28cf-67d124fb62c0"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a54e820b-654c-d289-f188-11d672a9a217"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a5574f00-d065-186a-8630-9c6de44235eb"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a9035ef4-18c2-3e13-d5cc-99fc5e63ed31"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ace9a66b-d17d-2b50-17b3-67eb40dcf41a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ad012924-5237-6205-38fe-d48cedc33d4d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b2f04998-b570-47d2-8236-0756a4e219e2"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b4e93b8d-5dc3-24cf-da95-fb3403b3749e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b5e24ad4-8420-94b6-b8fc-9dc56e2a445a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bbe26c6f-807b-dba2-eef3-33cd059b3296"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bc79a3c2-f98e-9c5b-5c26-3072265353bc"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bde31dac-221c-d02b-fca6-51fa576f9b89"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bf70e113-e074-6b40-6534-fb4c1f9231d3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c175a900-6ce3-e698-94df-f33b4ed6bb50"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c1a0d8cb-6b48-74ec-9919-d577f704c514"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c24d281f-89aa-dd6c-c264-59d5b5ba0faa"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce25f18c-ed21-3f6b-8dd1-82406f135d61"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ceadb402-0bb1-ea6e-7525-16b7a2ba8317"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d138659a-966d-09d0-2309-f233f3f232d1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d5c2ad14-559d-e8a6-0702-d8c9f26af634"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ddaa2356-dbdd-cef7-9cde-69e9fe7387a8"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2609822-210e-b891-0a3b-4be9978968e8"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e296cb77-368f-3b74-5b92-300a6e222336"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2b9095b-3271-9fb3-b450-3c31cf89f018"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e4d49503-81db-8555-9c3e-63285b5fb0e3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e71d339d-af13-2cae-30f1-389d04db8597"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e891d17c-d8a0-4681-2e67-c2535102b824"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f22816d7-77fb-13d5-6c4a-f0a724d75f2d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f71bec3e-d4d4-0866-0ed8-278392b16cc5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f7b250ee-574b-ccf7-78da-b6b63b865ed6"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fbf2ce76-86d4-1963-ba14-10b9ff31b052"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ff6fb9b4-3a44-22b2-9239-a335bc2aa23e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 431, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 17, 978, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 17, 978, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 17, 978, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 43, 18, 434, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$RGmZkrwZeCR9hNSlH/4L2.iSC8hTeVElGyhjqanaw7Zq/SOa1et2m");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerType",
                table: "WhatsAppTemplates");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000001-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000001-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000005"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000006"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000007"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000007-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000007-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000008-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000008-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000009-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000009-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000010-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000010-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000003"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000004"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000013-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000013-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000014-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000015-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000015-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000016-0000-0000-0000-000000000001"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000016-0000-0000-0000-000000000002"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 522, DateTimeKind.Utc).AddTicks(179));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("005f9b64-4d0a-6c82-947e-e57cfe738452"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("05195fdb-cd30-4561-9168-bbeb6f886a69"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0a025850-9197-728d-2039-14fa13c954ce"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0e32d4f0-e31f-1692-6763-43d06de36c0e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1def5b70-20cb-7e7b-b398-51bae635cb4c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2babdd1d-2af8-85c1-be3d-93660af5d9ca"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2cc74796-4a71-5342-2d08-a37e9154996d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("32c5144e-c485-1183-98b8-403fbb277ebd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("35837f97-2623-ab10-0cbd-370671326f89"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("430af60c-cfb8-d59a-e833-8cd110ff7f06"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("43fe5635-7b4d-ab3d-a51e-3559aced41c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("45a0ab26-2f4b-7f21-a073-171f907faf77"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("477c3b9c-bdb3-a703-0b56-f6516dcfc7e5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4d02d95c-4aab-3930-8c9f-b46c21d1be4e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5334b704-3d25-6b96-517f-6443fb11c414"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("55690f65-12c3-9922-c4b1-47b34caedde2"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("557952bd-50d0-d062-f35e-7fe6224bd482"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("566928b3-00dc-fb90-39b7-af77eb0f35b1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5f9b5de3-e563-a2d9-672a-91816ba23dd6"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("63139b75-fbfc-2bcf-36d2-43bc185b9a05"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6604e362-f7af-bb42-e9e1-448a76c8e188"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6a745601-638c-730b-cda2-d7a67e9f5a16"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6b5ac608-95b1-cd3e-d869-0fdf79a09fd0"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6b6ec0d1-eb14-8d88-e615-78e6fe6177bf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("70ec0ef2-a390-97c2-76ef-1678f6cf8fbf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7297595c-0a29-eb0d-19be-c531ee673c5b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("72bf7e60-3016-f5ff-ec42-ddfc1e27acfc"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7347301a-7584-8075-4aab-788b19078a62"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("78615f0f-b666-f010-e57b-dfb05c66f035"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("78b53d72-0ba7-6249-68a8-bedd284304f4"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7916078f-5d85-86e1-8747-b5dd207f40b5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("82cdc680-60c7-ca13-1fba-0c1725b381fa"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8680abc3-7a3f-fc42-ec4b-282165955f1c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("86f4523e-677d-889c-8e00-7ee5a50d94c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("89d0bc79-33f4-2a4d-f951-da329c328c20"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8ef92960-4f9f-70c2-58b7-a0fc32743d95"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("904ee737-7848-bded-ec47-ed74c6b3b5e5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("9cbf51af-d816-7ca8-28cf-67d124fb62c0"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a54e820b-654c-d289-f188-11d672a9a217"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a5574f00-d065-186a-8630-9c6de44235eb"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a9035ef4-18c2-3e13-d5cc-99fc5e63ed31"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ace9a66b-d17d-2b50-17b3-67eb40dcf41a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ad012924-5237-6205-38fe-d48cedc33d4d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b2f04998-b570-47d2-8236-0756a4e219e2"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b4e93b8d-5dc3-24cf-da95-fb3403b3749e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b5e24ad4-8420-94b6-b8fc-9dc56e2a445a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bbe26c6f-807b-dba2-eef3-33cd059b3296"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bc79a3c2-f98e-9c5b-5c26-3072265353bc"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bde31dac-221c-d02b-fca6-51fa576f9b89"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bf70e113-e074-6b40-6534-fb4c1f9231d3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c175a900-6ce3-e698-94df-f33b4ed6bb50"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c1a0d8cb-6b48-74ec-9919-d577f704c514"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c24d281f-89aa-dd6c-c264-59d5b5ba0faa"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce25f18c-ed21-3f6b-8dd1-82406f135d61"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ceadb402-0bb1-ea6e-7525-16b7a2ba8317"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d138659a-966d-09d0-2309-f233f3f232d1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d5c2ad14-559d-e8a6-0702-d8c9f26af634"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ddaa2356-dbdd-cef7-9cde-69e9fe7387a8"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2609822-210e-b891-0a3b-4be9978968e8"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e296cb77-368f-3b74-5b92-300a6e222336"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2b9095b-3271-9fb3-b450-3c31cf89f018"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e4d49503-81db-8555-9c3e-63285b5fb0e3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e71d339d-af13-2cae-30f1-389d04db8597"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e891d17c-d8a0-4681-2e67-c2535102b824"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f22816d7-77fb-13d5-6c4a-f0a724d75f2d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f71bec3e-d4d4-0866-0ed8-278392b16cc5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f7b250ee-574b-ccf7-78da-b6b63b865ed6"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fbf2ce76-86d4-1963-ba14-10b9ff31b052"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ff6fb9b4-3a44-22b2-9239-a335bc2aa23e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 524, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 276, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 276, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 276, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 55, 527, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$P99M3nZQVBhPhpuVJaspOu8IrzY0YZlY.lIbDt4.PmFmllyCxt3Om");
        }
    }
}
