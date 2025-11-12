using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class membermemberShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberMembership",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberMembership", x => x.MembershipId);
                    table.ForeignKey(
                        name: "FK_MemberMembership_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberMembership_MembershipPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "MembershipPlans",
                        principalColumn: "PlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 11, 19, 15, 19, 652, DateTimeKind.Utc).AddTicks(8338));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 11, 19, 15, 19, 652, DateTimeKind.Utc).AddTicks(8341));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 11, 19, 15, 19, 652, DateTimeKind.Utc).AddTicks(8343));

            migrationBuilder.CreateIndex(
                name: "IX_MemberMembership_MemberId",
                table: "MemberMembership",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberMembership_PlanId",
                table: "MemberMembership",
                column: "PlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberMembership");

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 11, 19, 1, 31, 351, DateTimeKind.Utc).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 11, 19, 1, 31, 351, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "MembershipPlans",
                keyColumn: "PlanId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 11, 19, 1, 31, 351, DateTimeKind.Utc).AddTicks(224));
        }
    }
}
