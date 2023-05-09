using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class UpdateFailFKRestaurantMenuCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestauranteId",
                table: "MenuCategories");

            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "MenuCategories",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RestaurantId",
                table: "MenuCategories",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "RestauranteId",
                table: "MenuCategories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
