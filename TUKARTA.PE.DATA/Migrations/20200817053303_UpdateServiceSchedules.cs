using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class UpdateServiceSchedules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceSchedules",
                columns: table => new
                {
                    RestaurantId = table.Column<Guid>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    OpeningTime = table.Column<TimeSpan>(nullable: false),
                    ClosingTime = table.Column<TimeSpan>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    SecondOpeningTime = table.Column<TimeSpan>(nullable: true),
                    SecondClosingTime = table.Column<TimeSpan>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSchedules", x => new { x.RestaurantId, x.Day });
                    table.ForeignKey(
                        name: "FK_ServiceSchedules_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceSchedules");
        }
    }
}
