using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddZKTecoModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceMemberMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceEnrollmentId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BiometricType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FingerIndex = table.Column<int>(type: "int", nullable: true),
                    EnrolledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMemberMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceMemberMappings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SyncAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMemberMappings_DeviceEnrollmentId_BiometricType",
                table: "DeviceMemberMappings",
                columns: new[] { "DeviceEnrollmentId", "BiometricType" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMemberMappings_MemberId_BiometricType",
                table: "DeviceMemberMappings",
                columns: new[] { "MemberId", "BiometricType" });

            migrationBuilder.CreateIndex(
                name: "IX_SyncAuditLogs_CreatedAt",
                table: "SyncAuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SyncAuditLogs_EventType",
                table: "SyncAuditLogs",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_SyncAuditLogs_Status",
                table: "SyncAuditLogs",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceMemberMappings");

            migrationBuilder.DropTable(
                name: "SyncAuditLogs");
        }
    }
}
