using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class updateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TrainerProfile",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TrainerProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "TrainerProfile",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MemberProfiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "MemberProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "MemberProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 12, 29, 476, DateTimeKind.Utc).AddTicks(9246));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 12, 29, 476, DateTimeKind.Utc).AddTicks(9249));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 15, 9, 12, 29, 476, DateTimeKind.Utc).AddTicks(9251));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TrainerProfile");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TrainerProfile");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "TrainerProfile");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MemberProfiles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "MemberProfiles");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "MemberProfiles");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Feedbacks");

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
    }
}
