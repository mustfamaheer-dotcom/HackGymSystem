using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataMigrations
{
    /// <inheritdoc />
    public partial class AddPaymentToDailySessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "DailySessions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "DailySessions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "DailySessions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "PlanId",
                table: "DailySessions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingBalance",
                table: "DailySessions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_DailySessions_PlanId",
                table: "DailySessions",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailySessions_MembershipPlans_PlanId",
                table: "DailySessions",
                column: "PlanId",
                principalTable: "MembershipPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailySessions_MembershipPlans_PlanId",
                table: "DailySessions");

            migrationBuilder.DropIndex(
                name: "IX_DailySessions_PlanId",
                table: "DailySessions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "DailySessions");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "DailySessions");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "DailySessions");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "DailySessions");

            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "DailySessions");
        }
    }
}
