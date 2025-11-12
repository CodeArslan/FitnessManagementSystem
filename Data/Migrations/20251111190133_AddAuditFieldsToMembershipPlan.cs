using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFieldsToMembershipPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.PlanId);
                });

            migrationBuilder.InsertData(
                table: "MembershipPlans",
                columns: new[] { "PlanId", "CreatedAt", "DeletedAt", "Description", "DurationMonths", "IsActive", "PlanName", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 11, 19, 1, 31, 351, DateTimeKind.Utc).AddTicks(218), null, "Full-year plan with trainer and diet support", 12, true, "Gold", 12000m, null },
                    { 2, new DateTime(2025, 11, 11, 19, 1, 31, 351, DateTimeKind.Utc).AddTicks(221), null, "Half-year plan with trainer support", 6, true, "Silver", 7000m, null },
                    { 3, new DateTime(2025, 11, 11, 19, 1, 31, 351, DateTimeKind.Utc).AddTicks(224), null, "Quarterly plan for new members", 3, true, "Bronze", 4000m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MembershipPlans");
        }
    }
}
