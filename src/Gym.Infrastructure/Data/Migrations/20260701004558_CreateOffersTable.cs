using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gym.Infrastructure.Data.Migrations
{
    public partial class CreateOffersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_Offers_IsActive",
                table: "Offers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_LinkedPackageId",
                table: "Offers",
                column: "LinkedPackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Offers");
        }
    }
}
