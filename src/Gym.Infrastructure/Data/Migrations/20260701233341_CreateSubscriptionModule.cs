using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateSubscriptionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TotalSubscriptionValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FreezeStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FreezeEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalFreezeDays = table.Column<int>(type: "int", nullable: false),
                    AdminSignature = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_MembershipPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "MembershipPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionFreezeHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FreezeStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FreezeEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FreezeDays = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UnfreezeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionFreezeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionFreezeHistories_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RunningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayments_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayments_Users_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTransactionLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PerformedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTransactionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionTransactionLogs_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionTransactionLogs_Users_PerformedBy",
                        column: x => x.PerformedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7682));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7650));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7687));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7683));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7652));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7683));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7676));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7677));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7688));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7684));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7679));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7685));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7680));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7647));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 57, DateTimeKind.Utc).AddTicks(7686));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("38e68501-6aa4-2723-251e-83ab85145eda"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("411c0941-f12c-e117-e542-7561547fae68"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("618876f6-daa1-fc83-c047-480bf88c794e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 33, 39, 58, DateTimeKind.Utc).AddTicks(5526));

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

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionFreezeHistories_SubscriptionId",
                table: "SubscriptionFreezeHistories",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayments_EmployeeId",
                table: "SubscriptionPayments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayments_SubscriptionId",
                table: "SubscriptionPayments",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_MemberId_Status",
                table: "Subscriptions",
                columns: new[] { "MemberId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_OfferId",
                table: "Subscriptions",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ReceiptNumber",
                table: "Subscriptions",
                column: "ReceiptNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_Status",
                table: "Subscriptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactionLogs_PerformedBy",
                table: "SubscriptionTransactionLogs",
                column: "PerformedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionTransactionLogs_SubscriptionId",
                table: "SubscriptionTransactionLogs",
                column: "SubscriptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionFreezeHistories");

            migrationBuilder.DropTable(
                name: "SubscriptionPayments");

            migrationBuilder.DropTable(
                name: "SubscriptionTransactionLogs");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a3b4c5d6-e7f8-9012-a345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6003));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a7b8c9d0-e1f2-3456-a789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(5997));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("a9b0c1d2-e3f4-5678-a901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6008));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b4c5d6e7-f8a9-0123-b456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6004));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("b8c9d0e1-f2a3-4567-b890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(5998));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c5d6e7f8-a9b0-1234-c567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6004));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("c9d0e1f2-a3b4-5678-c901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(5999));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d0e1f2a3-b4c5-6789-d012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d1e2f3a4-b5c6-7890-d123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6008));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("d6e7f8a9-b0c1-2345-d678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6005));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e1f2a3b4-c5d6-7890-e123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6001));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("e7f8a9b0-c1d2-3456-e789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6006));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f2a3b4c5-d6e7-8901-f234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f6a7b8c9-d0e1-2345-f678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(5995));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("f8a9b0c1-d2e3-4567-f890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 365, DateTimeKind.Utc).AddTicks(6007));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("0cd25d71-346b-3908-42cf-d84115ab6e8b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("12205256-ff83-d1a7-ca36-ef013e4348dd"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("18ca3a8f-4030-37b6-bb6c-51d7f78326bf"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("1a447bea-0884-a7b8-5c95-9aa22876b650"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("37fafc7e-cec4-f1e7-0639-0769adc78e8c"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("38e68501-6aa4-2723-251e-83ab85145eda"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("411c0941-f12c-e117-e542-7561547fae68"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("578d3c75-7400-e248-983f-cb4d4f13fdd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("618876f6-daa1-fc83-c047-480bf88c794e"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7b9944e2-4c52-0d53-1edd-ccd1d7316518"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7d73a719-0bf4-f70e-384e-ef6a71bdc88f"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("7dc649b3-0e4d-a02d-25b2-1533c6da1485"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("94e3232d-330d-080b-3968-8c0b0422abd3"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("95403e61-b5ef-2b2e-e6db-cb2875fdd651"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("97577489-44d4-9fca-3e5a-a7ff99c25f60"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("be8fe1d9-d5f7-fc0d-1c62-f23ec408ee0d"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c01cc128-784d-f6dd-8543-496e731fa31a"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("c4522deb-c1b9-9234-005e-bdb3b1c5d30b"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("ce45517e-5f0e-7330-9534-bae4dcee4905"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("d487e031-bfa2-73dc-4031-273dbfae68c5"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("dde6c68e-c7ec-19c2-65a8-d1c9abf4dbd1"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("df7c114a-08b2-ca57-d10a-9458a9126cc7"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("f6d95421-adde-78dc-a3e8-ada50506e534"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("faed05e8-a7e0-30e1-7f2f-3f59a113fd18"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumn: "Id",
                keyValue: new Guid("fb0338fb-fc95-ac51-0a37-7cc90b17ec50"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 366, DateTimeKind.Utc).AddTicks(3870));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 36, 951, DateTimeKind.Utc).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 36, 951, DateTimeKind.Utc).AddTicks(8352));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 36, 951, DateTimeKind.Utc).AddTicks(8358));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("b6c7d8e9-f0a1-2345-b678-901234567890"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("c7d8e9f0-a1b2-3456-c789-012345678901"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("d8e9f0a1-b2c3-4567-d890-123456789012"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"),
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 23, 19, 37, 367, DateTimeKind.Utc).AddTicks(8691));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"),
                column: "PasswordHash",
                value: "$2a$11$T5XPM1VTYOweLRV/DdLmjemlt3AWgoYRCA1z58g6mH49zuA4gkCwK");
        }
    }
}
