using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class setiembre1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Promotions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PromotionId",
                table: "Dishes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_PromotionId",
                table: "Dishes",
                column: "PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Promotions_PromotionId",
                table: "Dishes",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Promotions_PromotionId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_PromotionId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Dishes");
        }
    }
}
