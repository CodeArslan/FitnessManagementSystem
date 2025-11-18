using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class createFeedbacktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                });

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 9, 50, 66, DateTimeKind.Utc).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 9, 50, 66, DateTimeKind.Utc).AddTicks(3807));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 9, 50, 66, DateTimeKind.Utc).AddTicks(3810));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 4, 19, 638, DateTimeKind.Utc).AddTicks(2045));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 4, 19, 638, DateTimeKind.Utc).AddTicks(2048));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 4, 19, 638, DateTimeKind.Utc).AddTicks(2050));
        }
    }
}
