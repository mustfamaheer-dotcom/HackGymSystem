using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRolePermissionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Module",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PermissionAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPermissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionAuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionAuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MessageBody = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppTemplates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d1000001-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View main dashboard", "Dashboard", "Dashboard.View", null },
                    { new Guid("d1000001-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View revenue dashboard", "Dashboard", "Dashboard.Revenue", null },
                    { new Guid("d1000002-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View members list", "Members", "Members.View", null },
                    { new Guid("d1000002-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create new members", "Members", "Members.Create", null },
                    { new Guid("d1000002-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit existing members", "Members", "Members.Edit", null },
                    { new Guid("d1000002-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Delete members", "Members", "Members.Delete", null },
                    { new Guid("d1000003-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View plans", "Plans", "Plans.View", null },
                    { new Guid("d1000003-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create plans", "Plans", "Plans.Create", null },
                    { new Guid("d1000003-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit plans", "Plans", "Plans.Edit", null },
                    { new Guid("d1000003-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Delete plans", "Plans", "Plans.Delete", null },
                    { new Guid("d1000004-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View offers", "Offers", "Offers.View", null },
                    { new Guid("d1000004-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create offers", "Offers", "Offers.Create", null },
                    { new Guid("d1000004-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit offers", "Offers", "Offers.Edit", null },
                    { new Guid("d1000004-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Delete offers", "Offers", "Offers.Delete", null },
                    { new Guid("d1000005-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View subscriptions", "Subscriptions", "Subscriptions.View", null },
                    { new Guid("d1000005-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create subscriptions", "Subscriptions", "Subscriptions.Create", null },
                    { new Guid("d1000005-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit subscriptions", "Subscriptions", "Subscriptions.Edit", null },
                    { new Guid("d1000005-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Freeze subscriptions", "Subscriptions", "Subscriptions.Freeze", null },
                    { new Guid("d1000005-0000-0000-0000-000000000005"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Unfreeze subscriptions", "Subscriptions", "Subscriptions.Unfreeze", null },
                    { new Guid("d1000005-0000-0000-0000-000000000006"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Renew subscriptions", "Subscriptions", "Subscriptions.Renew", null },
                    { new Guid("d1000005-0000-0000-0000-000000000007"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View payment history", "Subscriptions", "Subscriptions.PaymentHistory", null },
                    { new Guid("d1000006-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View leads", "Leads", "Leads.View", null },
                    { new Guid("d1000006-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create leads", "Leads", "Leads.Create", null },
                    { new Guid("d1000006-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit leads", "Leads", "Leads.Edit", null },
                    { new Guid("d1000006-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Convert leads to members", "Leads", "Leads.Convert", null },
                    { new Guid("d1000007-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View attendance", "Attendance", "Attendance.View", null },
                    { new Guid("d1000007-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Export attendance", "Attendance", "Attendance.Export", null },
                    { new Guid("d1000008-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Send WhatsApp messages", "WhatsApp", "WhatsApp.Send", null },
                    { new Guid("d1000008-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Manual broadcast via WhatsApp", "WhatsApp", "WhatsApp.Broadcast", null },
                    { new Guid("d1000009-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Import data", "Import/Export", "ImportExport.Import", null },
                    { new Guid("d1000009-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Export data", "Import/Export", "ImportExport.Export", null },
                    { new Guid("d1000010-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View settings", "Settings", "Settings.View", null },
                    { new Guid("d1000010-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit settings", "Settings", "Settings.Edit", null },
                    { new Guid("d1000011-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View users", "User Management", "Users.View", null },
                    { new Guid("d1000011-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create users", "User Management", "Users.Create", null },
                    { new Guid("d1000011-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit users", "User Management", "Users.Edit", null },
                    { new Guid("d1000011-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Delete users", "User Management", "Users.Delete", null },
                    { new Guid("d1000012-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View roles & permissions", "Roles & Permissions", "Roles.View", null },
                    { new Guid("d1000012-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Create roles", "Roles & Permissions", "Roles.Create", null },
                    { new Guid("d1000012-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Edit roles", "Roles & Permissions", "Roles.Edit", null },
                    { new Guid("d1000012-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Delete roles", "Roles & Permissions", "Roles.Delete", null },
                    { new Guid("d1000013-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View devices", "Devices", "Devices.View", null },
                    { new Guid("d1000013-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Manage devices", "Devices", "Devices.Manage", null },
                    { new Guid("d1000014-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Manage backups", "Backup", "Backup.Manage", null },
                    { new Guid("d1000015-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View notifications", "Notifications", "Notifications.View", null },
                    { new Guid("d1000015-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Send notifications", "Notifications", "Notifications.Send", null },
                    { new Guid("d1000016-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "View memberships", "Memberships", "Memberships.View", null },
                    { new Guid("d1000016-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 0, 21, 54, 499, DateTimeKind.Utc).AddTicks(2200), "Manage memberships", "Memberships", "Memberships.Manage", null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                columns: new[] { "CreatedAt", "IsActive" },
                values: new object[] { new DateTime(2026, 7, 2, 0, 21, 53, 991, DateTimeKind.Utc).AddTicks(1928), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                columns: new[] { "CreatedAt", "IsActive" },
                values: new object[] { new DateTime(2026, 7, 2, 0, 21, 53, 991, DateTimeKind.Utc).AddTicks(1928), true });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                columns: new[] { "CreatedAt", "IsActive" },
                values: new object[] { new DateTime(2026, 7, 2, 0, 21, 53, 991, DateTimeKind.Utc).AddTicks(1928), false });

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 0, 21, 54, 506, DateTimeKind.Utc).AddTicks(1853));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$UwdzwSSzWIuGgiWmmhFNseBNrLh6GgbrXIhfdsMiZZEkmN5LRPQg.");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("005f9b64-4d0a-6c82-947e-e57cfe738452"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000012-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("05195fdb-cd30-4561-9168-bbeb6f886a69"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000003"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("0a025850-9197-728d-2039-14fa13c954ce"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000004"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("0e32d4f0-e31f-1692-6763-43d06de36c0e"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000012-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("1def5b70-20cb-7e7b-b398-51bae635cb4c"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000011-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("2babdd1d-2af8-85c1-be3d-93660af5d9ca"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000001-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("2cc74796-4a71-5342-2d08-a37e9154996d"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("32c5144e-c485-1183-98b8-403fbb277ebd"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000016-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("35837f97-2623-ab10-0cbd-370671326f89"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000015-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("430af60c-cfb8-d59a-e833-8cd110ff7f06"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000004"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("43fe5635-7b4d-ab3d-a51e-3559aced41c5"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("45a0ab26-2f4b-7f21-a073-171f907faf77"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("477c3b9c-bdb3-a703-0b56-f6516dcfc7e5"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000009-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000001-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("4d02d95c-4aab-3930-8c9f-b46c21d1be4e"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("5334b704-3d25-6b96-517f-6443fb11c414"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("55690f65-12c3-9922-c4b1-47b34caedde2"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000003"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("557952bd-50d0-d062-f35e-7fe6224bd482"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("566928b3-00dc-fb90-39b7-af77eb0f35b1"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("5f9b5de3-e563-a2d9-672a-91816ba23dd6"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("63139b75-fbfc-2bcf-36d2-43bc185b9a05"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000003-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6604e362-f7af-bb42-e9e1-448a76c8e188"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000008-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6a745601-638c-730b-cda2-d7a67e9f5a16"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000007"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("6b5ac608-95b1-cd3e-d869-0fdf79a09fd0"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000012-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6b6ec0d1-eb14-8d88-e615-78e6fe6177bf"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000007-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("70ec0ef2-a390-97c2-76ef-1678f6cf8fbf"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000001-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7297595c-0a29-eb0d-19be-c531ee673c5b"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000005"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("72bf7e60-3016-f5ff-ec42-ddfc1e27acfc"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000012-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7347301a-7584-8075-4aab-788b19078a62"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("78615f0f-b666-f010-e57b-dfb05c66f035"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000001-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("78b53d72-0ba7-6249-68a8-bedd284304f4"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000010-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7916078f-5d85-86e1-8747-b5dd207f40b5"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("82cdc680-60c7-ca13-1fba-0c1725b381fa"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000009-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("8680abc3-7a3f-fc42-ec4b-282165955f1c"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("86f4523e-677d-889c-8e00-7ee5a50d94c5"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("89d0bc79-33f4-2a4d-f951-da329c328c20"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000016-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("8ef92960-4f9f-70c2-58b7-a0fc32743d95"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000009-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("904ee737-7848-bded-ec47-ed74c6b3b5e5"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000015-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("9cbf51af-d816-7ca8-28cf-67d124fb62c0"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("a54e820b-654c-d289-f188-11d672a9a217"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000010-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("a5574f00-d065-186a-8630-9c6de44235eb"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000016-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("a9035ef4-18c2-3e13-d5cc-99fc5e63ed31"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("ace9a66b-d17d-2b50-17b3-67eb40dcf41a"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000011-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ad012924-5237-6205-38fe-d48cedc33d4d"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000003-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b2f04998-b570-47d2-8236-0756a4e219e2"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000003-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b4e93b8d-5dc3-24cf-da95-fb3403b3749e"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000008-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b5e24ad4-8420-94b6-b8fc-9dc56e2a445a"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000004-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000007"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bbe26c6f-807b-dba2-eef3-33cd059b3296"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000004-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bc79a3c2-f98e-9c5b-5c26-3072265353bc"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000014-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bde31dac-221c-d02b-fca6-51fa576f9b89"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000008-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000015-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bf70e113-e074-6b40-6534-fb4c1f9231d3"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000013-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c175a900-6ce3-e698-94df-f33b4ed6bb50"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000007-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c1a0d8cb-6b48-74ec-9919-d577f704c514"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000011-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c24d281f-89aa-dd6c-c264-59d5b5ba0faa"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000011-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ce25f18c-ed21-3f6b-8dd1-82406f135d61"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000005"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ceadb402-0bb1-ea6e-7525-16b7a2ba8317"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000004-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d138659a-966d-09d0-2309-f233f3f232d1"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000006"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("d5c2ad14-559d-e8a6-0702-d8c9f26af634"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000005-0000-0000-0000-000000000006"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ddaa2356-dbdd-cef7-9cde-69e9fe7387a8"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000003-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000015-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("e2609822-210e-b891-0a3b-4be9978968e8"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e296cb77-368f-3b74-5b92-300a6e222336"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000006-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("e2b9095b-3271-9fb3-b450-3c31cf89f018"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000013-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e4d49503-81db-8555-9c3e-63285b5fb0e3"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000015-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("e71d339d-af13-2cae-30f1-389d04db8597"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("e891d17c-d8a0-4681-2e67-c2535102b824"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000004-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("f22816d7-77fb-13d5-6c4a-f0a724d75f2d"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000002-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("f71bec3e-d4d4-0866-0ed8-278392b16cc5"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000007-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("f7b250ee-574b-ccf7-78da-b6b63b865ed6"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000007-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("fbf2ce76-86d4-1963-ba14-10b9ff31b052"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000009-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ff6fb9b4-3a44-22b2-9239-a335bc2aa23e"), new DateTime(2026, 7, 2, 0, 21, 54, 502, DateTimeKind.Utc).AddTicks(9626), new Guid("d1000007-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionAuditLogs_UserId",
                table: "PermissionAuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsAppTemplates_Name",
                table: "WhatsAppTemplates",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionAuditLogs");

            migrationBuilder.DropTable(
                name: "WhatsAppTemplates");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("005f9b64-4d0a-6c82-947e-e57cfe738452"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("05195fdb-cd30-4561-9168-bbeb6f886a69"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0a025850-9197-728d-2039-14fa13c954ce"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0e32d4f0-e31f-1692-6763-43d06de36c0e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1def5b70-20cb-7e7b-b398-51bae635cb4c"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2babdd1d-2af8-85c1-be3d-93660af5d9ca"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("2cc74796-4a71-5342-2d08-a37e9154996d"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("32c5144e-c485-1183-98b8-403fbb277ebd"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("35837f97-2623-ab10-0cbd-370671326f89"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("3c0e6bfe-9913-56af-20cb-cc7f4e36a4a6"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("430af60c-cfb8-d59a-e833-8cd110ff7f06"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("43fe5635-7b4d-ab3d-a51e-3559aced41c5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("45a0ab26-2f4b-7f21-a073-171f907faf77"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("477c3b9c-bdb3-a703-0b56-f6516dcfc7e5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("4d02d95c-4aab-3930-8c9f-b46c21d1be4e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5334b704-3d25-6b96-517f-6443fb11c414"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("55690f65-12c3-9922-c4b1-47b34caedde2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("557952bd-50d0-d062-f35e-7fe6224bd482"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("566928b3-00dc-fb90-39b7-af77eb0f35b1"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("5f9b5de3-e563-a2d9-672a-91816ba23dd6"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("63139b75-fbfc-2bcf-36d2-43bc185b9a05"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6604e362-f7af-bb42-e9e1-448a76c8e188"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6a745601-638c-730b-cda2-d7a67e9f5a16"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6b5ac608-95b1-cd3e-d869-0fdf79a09fd0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("6b6ec0d1-eb14-8d88-e615-78e6fe6177bf"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("70ec0ef2-a390-97c2-76ef-1678f6cf8fbf"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7297595c-0a29-eb0d-19be-c531ee673c5b"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("72bf7e60-3016-f5ff-ec42-ddfc1e27acfc"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7347301a-7584-8075-4aab-788b19078a62"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("78615f0f-b666-f010-e57b-dfb05c66f035"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("78b53d72-0ba7-6249-68a8-bedd284304f4"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7916078f-5d85-86e1-8747-b5dd207f40b5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("82cdc680-60c7-ca13-1fba-0c1725b381fa"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8680abc3-7a3f-fc42-ec4b-282165955f1c"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("86f4523e-677d-889c-8e00-7ee5a50d94c5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("89d0bc79-33f4-2a4d-f951-da329c328c20"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("8ef92960-4f9f-70c2-58b7-a0fc32743d95"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("904ee737-7848-bded-ec47-ed74c6b3b5e5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("9cbf51af-d816-7ca8-28cf-67d124fb62c0"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a54e820b-654c-d289-f188-11d672a9a217"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a5574f00-d065-186a-8630-9c6de44235eb"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("a9035ef4-18c2-3e13-d5cc-99fc5e63ed31"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ace9a66b-d17d-2b50-17b3-67eb40dcf41a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ad012924-5237-6205-38fe-d48cedc33d4d"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b2f04998-b570-47d2-8236-0756a4e219e2"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b4e93b8d-5dc3-24cf-da95-fb3403b3749e"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b5e24ad4-8420-94b6-b8fc-9dc56e2a445a"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bbe26c6f-807b-dba2-eef3-33cd059b3296"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bc79a3c2-f98e-9c5b-5c26-3072265353bc"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bde31dac-221c-d02b-fca6-51fa576f9b89"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bf6ac31e-ca1d-b678-8e6e-3cc7b7dac946"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("bf70e113-e074-6b40-6534-fb4c1f9231d3"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c175a900-6ce3-e698-94df-f33b4ed6bb50"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c1a0d8cb-6b48-74ec-9919-d577f704c514"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c24d281f-89aa-dd6c-c264-59d5b5ba0faa"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce25f18c-ed21-3f6b-8dd1-82406f135d61"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ceadb402-0bb1-ea6e-7525-16b7a2ba8317"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d138659a-966d-09d0-2309-f233f3f232d1"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d5c2ad14-559d-e8a6-0702-d8c9f26af634"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ddaa2356-dbdd-cef7-9cde-69e9fe7387a8"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e02b85cd-9dd3-5a0e-0700-cd4e3f2d9ed4"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2609822-210e-b891-0a3b-4be9978968e8"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e296cb77-368f-3b74-5b92-300a6e222336"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e2b9095b-3271-9fb3-b450-3c31cf89f018"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e4d49503-81db-8555-9c3e-63285b5fb0e3"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e71d339d-af13-2cae-30f1-389d04db8597"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("e891d17c-d8a0-4681-2e67-c2535102b824"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f22816d7-77fb-13d5-6c4a-f0a724d75f2d"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f71bec3e-d4d4-0866-0ed8-278392b16cc5"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f7b250ee-574b-ccf7-78da-b6b63b865ed6"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fbf2ce76-86d4-1963-ba14-10b9ff31b052"));

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ff6fb9b4-3a44-22b2-9239-a335bc2aa23e"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000002-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000003-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000004-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000005-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000006-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000007-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000007-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000008-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000008-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000009-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000009-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000010-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000010-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000011-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000012-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000013-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000013-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000014-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000015-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000015-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000016-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1000016-0000-0000-0000-000000000002"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Module",
                table: "Permissions");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7682), "Process payments", "payments.write", null },
                    { new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7650), "Delete members", "members.delete", null },
                    { new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7687), "Manage offers", "offers.manage", null },
                    { new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7683), "View reports", "reports.read", null },
                    { new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7652), "View plans", "plans.read", null },
                    { new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7683), "View settings", "settings.read", null },
                    { new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7676), "Create/Edit plans", "plans.write", null },
                    { new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7677), "View attendance", "attendance.read", null },
                    { new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7688), "Manage users", "users.manage", null },
                    { new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7684), "Manage settings", "settings.write", null },
                    { new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7679), "Record attendance", "attendance.write", null },
                    { new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(6403), "View members", "members.read", null },
                    { new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7685), "Manage devices", "devices.manage", null },
                    { new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7680), "View payments", "payments.read", null },
                    { new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7647), "Create/Edit members", "members.write", null },
                    { new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7686), "Manage backups", "backup.manage", null }
                });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 38, 609, DateTimeKind.Utc).AddTicks(3396));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 38, 609, DateTimeKind.Utc).AddTicks(4715));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 38, 609, DateTimeKind.Utc).AddTicks(4717));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 60, DateTimeKind.Utc).AddTicks(565));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$uMHEZbu/qmOybTH6otB6eumV0F1Qvs5TKpOo/Kh9KJlIspYFpm/ay");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("38e68501-6aa4-2723-251e-83ab85145eda"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("411c0941-f12c-e117-e542-7561547fae68"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("618876f6-daa1-fc83-c047-480bf88c794e"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"), new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526), new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null }
                });
        }
    }
}
