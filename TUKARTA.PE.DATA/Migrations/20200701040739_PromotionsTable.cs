using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class PromotionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AvailableToCarryOut = table.Column<bool>(nullable: false),
                    AvailableToEatIn = table.Column<bool>(nullable: false),
                    AvailableForOrder = table.Column<bool>(nullable: false),
                    AvailableForBooking = table.Column<bool>(nullable: false),
                    AvailableForDelivery = table.Column<bool>(nullable: false),
                    ServiceType = table.Column<int>(nullable: false),
                    ExpirationDateTime = table.Column<DateTime>(nullable: false),
                    NewPrice = table.Column<double>(nullable: false),
                    DishId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_DishId",
                table: "Promotions",
                column: "DishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotions");
        }
    }
}
