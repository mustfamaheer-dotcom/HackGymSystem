using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Gym.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackupLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BackupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackupLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirmwareVersion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastConnectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    MaxVisits = table.Column<int>(type: "int", nullable: true),
                    FreezeDays = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsSystem = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsEncrypted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhatsAppTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MessageBody = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TriggerType = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsAppTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceLogs_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Company = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(5,1)", precision: 5, scale: 1, nullable: true),
                    HasDisease = table.Column<bool>(type: "bit", nullable: false),
                    DiseaseType = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReferralSource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubscriptionPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SubscriptionDurationMonths = table.Column<int>(type: "int", nullable: false),
                    FreeMonths = table.Column<int>(type: "int", nullable: false),
                    FreezeDays = table.Column<int>(type: "int", nullable: false),
                    SubscriptionStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubscriptionEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FingerprintDeviceId = table.Column<int>(type: "int", nullable: true),
                    MemberSignature = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    AdminSignature = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_MembershipPlans_PackageId",
                        column: x => x.PackageId,
                        principalTable: "MembershipPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LinkedPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OfferType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BonusMonths = table.Column<int>(type: "int", nullable: true),
                    BonusDays = table.Column<int>(type: "int", nullable: true),
                    OfferPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ExtraFreezeDays = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_MembershipPlans_LinkedPackageId",
                        column: x => x.LinkedPackageId,
                        principalTable: "MembershipPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    SyncStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendance_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Attendance_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedAt", "Description", "Module", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d1000001-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View main dashboard", "Dashboard", "Dashboard.View", null },
                    { new Guid("d1000001-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View revenue dashboard", "Dashboard", "Dashboard.Revenue", null },
                    { new Guid("d1000002-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View members list", "Members", "Members.View", null },
                    { new Guid("d1000002-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create new members", "Members", "Members.Create", null },
                    { new Guid("d1000002-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit existing members", "Members", "Members.Edit", null },
                    { new Guid("d1000002-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Delete members", "Members", "Members.Delete", null },
                    { new Guid("d1000003-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View plans", "Plans", "Plans.View", null },
                    { new Guid("d1000003-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create plans", "Plans", "Plans.Create", null },
                    { new Guid("d1000003-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit plans", "Plans", "Plans.Edit", null },
                    { new Guid("d1000003-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Delete plans", "Plans", "Plans.Delete", null },
                    { new Guid("d1000004-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View offers", "Offers", "Offers.View", null },
                    { new Guid("d1000004-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create offers", "Offers", "Offers.Create", null },
                    { new Guid("d1000004-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit offers", "Offers", "Offers.Edit", null },
                    { new Guid("d1000004-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Delete offers", "Offers", "Offers.Delete", null },
                    { new Guid("d1000005-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View subscriptions", "Subscriptions", "Subscriptions.View", null },
                    { new Guid("d1000005-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create subscriptions", "Subscriptions", "Subscriptions.Create", null },
                    { new Guid("d1000005-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit subscriptions", "Subscriptions", "Subscriptions.Edit", null },
                    { new Guid("d1000005-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Freeze subscriptions", "Subscriptions", "Subscriptions.Freeze", null },
                    { new Guid("d1000005-0000-0000-0000-000000000005"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Unfreeze subscriptions", "Subscriptions", "Subscriptions.Unfreeze", null },
                    { new Guid("d1000005-0000-0000-0000-000000000006"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Renew subscriptions", "Subscriptions", "Subscriptions.Renew", null },
                    { new Guid("d1000005-0000-0000-0000-000000000007"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View payment history", "Subscriptions", "Subscriptions.PaymentHistory", null },
                    { new Guid("d1000006-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View leads", "Leads", "Leads.View", null },
                    { new Guid("d1000006-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create leads", "Leads", "Leads.Create", null },
                    { new Guid("d1000006-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit leads", "Leads", "Leads.Edit", null },
                    { new Guid("d1000006-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Convert leads to members", "Leads", "Leads.Convert", null },
                    { new Guid("d1000007-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View attendance", "Attendance", "Attendance.View", null },
                    { new Guid("d1000007-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Export attendance", "Attendance", "Attendance.Export", null },
                    { new Guid("d1000008-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Send WhatsApp messages", "WhatsApp", "WhatsApp.Send", null },
                    { new Guid("d1000008-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Manual broadcast via WhatsApp", "WhatsApp", "WhatsApp.Broadcast", null },
                    { new Guid("d1000009-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Import data", "Import/Export", "ImportExport.Import", null },
                    { new Guid("d1000009-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Export data", "Import/Export", "ImportExport.Export", null },
                    { new Guid("d1000010-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View settings", "Settings", "Settings.View", null },
                    { new Guid("d1000010-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit settings", "Settings", "Settings.Edit", null },
                    { new Guid("d1000011-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View users", "User Management", "Users.View", null },
                    { new Guid("d1000011-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create users", "User Management", "Users.Create", null },
                    { new Guid("d1000011-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit users", "User Management", "Users.Edit", null },
                    { new Guid("d1000011-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Delete users", "User Management", "Users.Delete", null },
                    { new Guid("d1000012-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View roles & permissions", "Roles & Permissions", "Roles.View", null },
                    { new Guid("d1000012-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Create roles", "Roles & Permissions", "Roles.Create", null },
                    { new Guid("d1000012-0000-0000-0000-000000000003"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Edit roles", "Roles & Permissions", "Roles.Edit", null },
                    { new Guid("d1000012-0000-0000-0000-000000000004"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Delete roles", "Roles & Permissions", "Roles.Delete", null },
                    { new Guid("d1000013-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "View devices", "Devices", "Devices.View", null },
                    { new Guid("d1000013-0000-0000-0000-000000000002"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Manage devices", "Devices", "Devices.Manage", null },
                    { new Guid("d1000014-0000-0000-0000-000000000001"), new DateTime(2026, 7, 2, 12, 37, 50, 620, DateTimeKind.Utc).AddTicks(5242), "Manage backups", "Backup", "Backup.Manage", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "IsSystem", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), new DateTime(2026, 7, 2, 12, 37, 49, 727, DateTimeKind.Utc).AddTicks(3457), "Full system access", true, true, "Owner", null },
                    { new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), new DateTime(2026, 7, 2, 12, 37, 49, 727, DateTimeKind.Utc).AddTicks(3457), "Front desk operations", true, true, "Receptionist", null },
                    { new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), new DateTime(2026, 7, 2, 12, 37, 49, 727, DateTimeKind.Utc).AddTicks(3457), "Trainer limited access", false, true, "Trainer", null }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "CreatedAt", "Description", "Group", "IsEncrypted", "Key", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { new Guid("a5b6c7d8-e9f0-1234-a567-890123456789"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "Gym closing time", "General", false, "WorkingHoursEnd", null, "22:00" },
                    { new Guid("b0c1d2e3-f4a5-6789-b012-345678901234"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "Gym display name", "General", false, "GymName", null, "My Gym" },
                    { new Guid("c1d2e3f4-a5b6-7890-c123-456789012345"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "ZKTeco MB2000 IP address", "Device", false, "DeviceIP", null, "192.168.1.201" },
                    { new Guid("d2e3f4a5-b6c7-8901-d234-567890123456"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "ZKTeco MB2000 port", "Device", false, "DevicePort", null, "4370" },
                    { new Guid("e3f4a5b6-c7d8-9012-e345-678901234567"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "Database backup location", "Backup", false, "BackupPath", null, "C:\\Backups\\GymManagement" },
                    { new Guid("e9f0a1b2-c3d4-5678-e901-234567890123"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "Default currency symbol", "General", false, "DefaultCurrency", null, "EGP" },
                    { new Guid("f4a5b6c7-d8e9-0123-f456-789012345678"), new DateTime(2026, 7, 2, 12, 37, 50, 638, DateTimeKind.Utc).AddTicks(4533), "Gym opening time", "General", false, "WorkingHoursStart", null, "08:00" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Id", "CreatedAt", "PermissionId", "RoleId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("005f9b64-4d0a-6c82-947e-e57cfe738452"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000012-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("05195fdb-cd30-4561-9168-bbeb6f886a69"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000003"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("0a025850-9197-728d-2039-14fa13c954ce"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000004"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("0e32d4f0-e31f-1692-6763-43d06de36c0e"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000012-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("1def5b70-20cb-7e7b-b398-51bae635cb4c"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000011-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("2babdd1d-2af8-85c1-be3d-93660af5d9ca"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000001-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("2cc74796-4a71-5342-2d08-a37e9154996d"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("35837f97-2623-ab10-0cbd-370671326f89"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("430af60c-cfb8-d59a-e833-8cd110ff7f06"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000004"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("43fe5635-7b4d-ab3d-a51e-3559aced41c5"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("45a0ab26-2f4b-7f21-a073-171f907faf77"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("477c3b9c-bdb3-a703-0b56-f6516dcfc7e5"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000009-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("48e05a34-1b30-1cdc-ca0d-974cb2c3ebd8"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000001-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("4d02d95c-4aab-3930-8c9f-b46c21d1be4e"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("5334b704-3d25-6b96-517f-6443fb11c414"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("55690f65-12c3-9922-c4b1-47b34caedde2"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000003"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("557952bd-50d0-d062-f35e-7fe6224bd482"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("566928b3-00dc-fb90-39b7-af77eb0f35b1"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("5f9b5de3-e563-a2d9-672a-91816ba23dd6"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("63139b75-fbfc-2bcf-36d2-43bc185b9a05"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000003-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6604e362-f7af-bb42-e9e1-448a76c8e188"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000008-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6a745601-638c-730b-cda2-d7a67e9f5a16"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000007"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("6b5ac608-95b1-cd3e-d869-0fdf79a09fd0"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000012-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("6b6ec0d1-eb14-8d88-e615-78e6fe6177bf"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000007-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("70ec0ef2-a390-97c2-76ef-1678f6cf8fbf"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000001-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7297595c-0a29-eb0d-19be-c531ee673c5b"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000005"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("72bf7e60-3016-f5ff-ec42-ddfc1e27acfc"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000012-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7347301a-7584-8075-4aab-788b19078a62"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("78615f0f-b666-f010-e57b-dfb05c66f035"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000001-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("78b53d72-0ba7-6249-68a8-bedd284304f4"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000010-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("7916078f-5d85-86e1-8747-b5dd207f40b5"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("82cdc680-60c7-ca13-1fba-0c1725b381fa"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("85bf1ab4-8b34-6fb0-9ad7-cbbd215d0949"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000009-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("8680abc3-7a3f-fc42-ec4b-282165955f1c"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("86f4523e-677d-889c-8e00-7ee5a50d94c5"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("8ef92960-4f9f-70c2-58b7-a0fc32743d95"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000009-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("9cbf51af-d816-7ca8-28cf-67d124fb62c0"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("a54e820b-654c-d289-f188-11d672a9a217"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000010-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("a9035ef4-18c2-3e13-d5cc-99fc5e63ed31"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("ace9a66b-d17d-2b50-17b3-67eb40dcf41a"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000011-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ad012924-5237-6205-38fe-d48cedc33d4d"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000003-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b2f04998-b570-47d2-8236-0756a4e219e2"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000003-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b4e93b8d-5dc3-24cf-da95-fb3403b3749e"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000008-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b5e24ad4-8420-94b6-b8fc-9dc56e2a445a"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000004-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("b94ea0da-6b9a-ea4f-6ac8-08b4e88272fe"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000007"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bbe26c6f-807b-dba2-eef3-33cd059b3296"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000004-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bc79a3c2-f98e-9c5b-5c26-3072265353bc"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000014-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("bde31dac-221c-d02b-fca6-51fa576f9b89"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000008-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("bf70e113-e074-6b40-6534-fb4c1f9231d3"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000013-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c175a900-6ce3-e698-94df-f33b4ed6bb50"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000007-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c1a0d8cb-6b48-74ec-9919-d577f704c514"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000011-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("c24d281f-89aa-dd6c-c264-59d5b5ba0faa"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000011-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ce25f18c-ed21-3f6b-8dd1-82406f135d61"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000005"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ceadb402-0bb1-ea6e-7525-16b7a2ba8317"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000004-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("d138659a-966d-09d0-2309-f233f3f232d1"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000006"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("d5c2ad14-559d-e8a6-0702-d8c9f26af634"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000005-0000-0000-0000-000000000006"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ddaa2356-dbdd-cef7-9cde-69e9fe7387a8"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000003-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e2609822-210e-b891-0a3b-4be9978968e8"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000003"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e296cb77-368f-3b74-5b92-300a6e222336"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000006-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("e2b9095b-3271-9fb3-b450-3c31cf89f018"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000013-0000-0000-0000-000000000002"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("e71d339d-af13-2cae-30f1-389d04db8597"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("e891d17c-d8a0-4681-2e67-c2535102b824"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000004-0000-0000-0000-000000000004"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("f22816d7-77fb-13d5-6c4a-f0a724d75f2d"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000002-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null },
                    { new Guid("f71bec3e-d4d4-0866-0ed8-278392b16cc5"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000007-0000-0000-0000-000000000002"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("f7b250ee-574b-ccf7-78da-b6b63b865ed6"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000007-0000-0000-0000-000000000001"), new Guid("b2c3d4e5-f6a7-8901-bcde-f12345678901"), null },
                    { new Guid("fbf2ce76-86d4-1963-ba14-10b9ff31b052"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000009-0000-0000-0000-000000000001"), new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null },
                    { new Guid("ff6fb9b4-3a44-22b2-9239-a335bc2aa23e"), new DateTime(2026, 7, 2, 12, 37, 50, 628, DateTimeKind.Utc).AddTicks(3640), new Guid("d1000007-0000-0000-0000-000000000001"), new Guid("c3d4e5f6-a7b8-9012-cdef-123456789012"), null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "Phone", "RefreshToken", "RefreshTokenExpiry", "RoleId", "UpdatedAt", "Username" },
                values: new object[] { new Guid("d4e5f6a7-b8c9-0123-def4-567890123456"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@gym.com", "System Administrator", true, null, "$2a$11$dkQ740bKq39kq9yVKqhBK.Re7tfLvFcD3DUwxMTc6eAjqD2EwZx2W", null, null, null, new Guid("a1b2c3d4-e5f6-7890-abcd-ef1234567890"), null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_DeviceId_Date",
                table: "Attendance",
                columns: new[] { "DeviceId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_MemberId_Date",
                table: "Attendance",
                columns: new[] { "MemberId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BackupLogs_BackupDate",
                table: "BackupLogs",
                column: "BackupDate");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogs_CreatedAt",
                table: "DeviceLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceLogs_DeviceId",
                table: "DeviceLogs",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Code",
                table: "Members",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_NationalId",
                table: "Members",
                column: "NationalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_PackageId",
                table: "Members",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ReceiptNumber",
                table: "Members",
                column: "ReceiptNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_IsActive",
                table: "Offers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_LinkedPackageId",
                table: "Offers",
                column: "LinkedPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionAuditLogs_UserId",
                table: "PermissionAuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId_PermissionId",
                table: "RolePermissions",
                columns: new[] { "RoleId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key",
                table: "Settings",
                column: "Key",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

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
                name: "Attendance");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BackupLogs");

            migrationBuilder.DropTable(
                name: "DeviceLogs");

            migrationBuilder.DropTable(
                name: "PermissionAuditLogs");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "SubscriptionFreezeHistories");

            migrationBuilder.DropTable(
                name: "SubscriptionPayments");

            migrationBuilder.DropTable(
                name: "SubscriptionTransactionLogs");

            migrationBuilder.DropTable(
                name: "WhatsAppTemplates");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "MembershipPlans");
        }
    }
}
