using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class RestaurantNewDeliveryFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DeliveryCostPerKilometer",
                table: "Restaurants",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MinimumAmount",
                table: "Restaurants",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ResponsibleName = table.Column<string>(nullable: false),
                    BusinessName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropColumn(
                name: "DeliveryCostPerKilometer",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MinimumAmount",
                table: "Restaurants");
        }
    }
}
