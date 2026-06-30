using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNewMemberEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("03eb6362-d558-4c52-abcf-d56fc0f6955c"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("04ccbdc5-1aeb-4e81-ac19-ab620c85e9b6"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("057302f5-3758-4d01-8e38-c991b59f2298"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("06b4b0d4-b1b1-41eb-ad43-9541ddefa59b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0af92da4-9a07-4290-a522-78dfbab5c81f"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("127002d8-cf6a-4035-bfb9-84cb3f51038c"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1893a9d6-ba62-4eb4-a070-92fc3951892f"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1ca3ec1a-826e-48e5-b5aa-e1a916ae1909"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("3dbc6a23-8010-4fde-add2-68769b0cbbf4"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("44c7afa1-a567-4652-b083-05ae13bfcecd"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5a22b771-294e-402f-8996-544c71a2bf35"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("64bb391f-27d3-49ea-8a1c-5a9f1cff948f"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("746cc333-2a27-419a-96db-41967d2f8324"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("764dca3f-41de-45db-a89d-5c20fc8b22e9"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("9d95fc93-62b9-43e4-85df-d48bae9db131"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c16606-294c-4be3-be61-bc3f63ed11e0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bd48abee-a0f4-46e4-b0ba-33d161c638af"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bd8b45a6-05d7-457a-9017-fb87524b1cf5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d77a3621-8dc1-4df1-959d-adf1ddc798de"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ea0b55ff-58a4-4a91-a4c7-f7219ab5f975"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ee3e2099-ec64-48e8-9afd-f461f035e6f5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f30ecd20-1572-4377-a981-47bf992fe5ac"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f8f4bfff-728b-45b5-ae2c-3b19387a27c0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fec91eab-19ad-4654-8117-f2ab2473f88a"));

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "DeviceUserId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "EmergencyContactPhone",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FaceId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Members",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "PhotoPath",
                table: "Members",
                newName: "DiseaseType");

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Members",
                newName: "SubscriptionStartDate");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Members",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "FingerprintId",
                table: "Members",
                newName: "FingerprintDeviceId");

            migrationBuilder.RenameColumn(
                name: "EmergencyContactName",
                table: "Members",
                newName: "ReferralSource");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Members",
                newName: "Company");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Members",
                newName: "ReceiptNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Members_Code",
                table: "Members",
                newName: "IX_Members_ReceiptNumber");

            migrationBuilder.AddColumn<string>(
                name: "AdminSignature",
                table: "Members",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FreeMonths",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FreezeDays",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasDisease",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MemberSignature",
                table: "Members",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Members",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageId",
                table: "Members",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Members",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Members",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingAmount",
                table: "Members",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionDurationMonths",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionEndDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "SubscriptionPrice",
                table: "Members",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Members",
                type: "decimal(5,1)",
                precision: 5,
                scale: 1,
                nullable: true);

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
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[] { new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new DateTime(2026, 6, 29, 22, 50, 43, 470, DateTimeKind.Utc).AddTicks(7198), "Manage users", "users.manage", null });

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

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[] { new Guid("cfd7c3f6-1bc8-4e62-97fd-b2e60c64aa3d"), new DateTime(2026, 6, 29, 22, 50, 43, 472, DateTimeKind.Utc).AddTicks(953), new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null });

            migrationBuilder.CreateIndex(
                name: "IX_Members_NationalId",
                table: "Members",
                column: "NationalId",
                unique: true,
                filter: "[NationalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PackageId",
                table: "Members",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_MembershipPlans_PackageId",
                table: "Members",
                column: "PackageId",
                principalTable: "MembershipPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_MembershipPlans_PackageId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_NationalId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_PackageId",
                table: "Members");

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

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"));

            migrationBuilder.DropColumn(
                name: "AdminSignature",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FreeMonths",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FreezeDays",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "HasDisease",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MemberSignature",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "RemainingAmount",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SubscriptionDurationMonths",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SubscriptionEndDate",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "SubscriptionPrice",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "SubscriptionStartDate",
                table: "Members",
                newName: "JoinDate");

            migrationBuilder.RenameColumn(
                name: "ReferralSource",
                table: "Members",
                newName: "EmergencyContactName");

            migrationBuilder.RenameColumn(
                name: "ReceiptNumber",
                table: "Members",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Members",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Members",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "FingerprintDeviceId",
                table: "Members",
                newName: "FingerprintId");

            migrationBuilder.RenameColumn(
                name: "DiseaseType",
                table: "Members",
                newName: "PhotoPath");

            migrationBuilder.RenameColumn(
                name: "Company",
                table: "Members",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Members_ReceiptNumber",
                table: "Members",
                newName: "IX_Members_Code");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Members",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeviceUserId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContactPhone",
                table: "Members",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaceId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Members",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Members",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Members",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "Members",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9083));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9101));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9084));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9079));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9085));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9080));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9081));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9086));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9082));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(7921));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9087));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9083));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 370, DateTimeKind.Utc).AddTicks(9100));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("03eb6362-d558-4c52-abcf-d56fc0f6955c"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("04ccbdc5-1aeb-4e81-ac19-ab620c85e9b6"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("057302f5-3758-4d01-8e38-c991b59f2298"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("06b4b0d4-b1b1-41eb-ad43-9541ddefa59b"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("0af92da4-9a07-4290-a522-78dfbab5c81f"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("127002d8-cf6a-4035-bfb9-84cb3f51038c"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("1893a9d6-ba62-4eb4-a070-92fc3951892f"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("1ca3ec1a-826e-48e5-b5aa-e1a916ae1909"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("3dbc6a23-8010-4fde-add2-68769b0cbbf4"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("44c7afa1-a567-4652-b083-05ae13bfcecd"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("5a22b771-294e-402f-8996-544c71a2bf35"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("64bb391f-27d3-49ea-8a1c-5a9f1cff948f"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("746cc333-2a27-419a-96db-41967d2f8324"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("764dca3f-41de-45db-a89d-5c20fc8b22e9"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("9d95fc93-62b9-43e4-85df-d48bae9db131"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("b8c16606-294c-4be3-be61-bc3f63ed11e0"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bd48abee-a0f4-46e4-b0ba-33d161c638af"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bd8b45a6-05d7-457a-9017-fb87524b1cf5"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d77a3621-8dc1-4df1-959d-adf1ddc798de"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("ea0b55ff-58a4-4a91-a4c7-f7219ab5f975"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("ee3e2099-ec64-48e8-9afd-f461f035e6f5"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("f30ecd20-1572-4377-a981-47bf992fe5ac"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("f8f4bfff-728b-45b5-ae2c-3b19387a27c0"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("fec91eab-19ad-4654-8117-f2ab2473f88a"), new DateTime(2026, 6, 21, 18, 0, 4, 371, DateTimeKind.Utc).AddTicks(6142), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 3, 944, DateTimeKind.Utc).AddTicks(1628));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 3, 944, DateTimeKind.Utc).AddTicks(2490));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 3, 944, DateTimeKind.Utc).AddTicks(2490));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 6, 21, 18, 0, 4, 372, DateTimeKind.Utc).AddTicks(4362));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$1brDlyE5gMcS/ugJbDUMRO.snjzcwW9lH.BQXxHaf5gfdzItjRgim");
        }
    }
}
