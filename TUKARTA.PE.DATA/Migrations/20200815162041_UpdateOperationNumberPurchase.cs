using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class UpdateOperationNumberPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OperationNumber",
                table: "Purchases",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                table: "PurchaseDetails",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationNumber",
                table: "Purchases");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseId",
                table: "PurchaseDetails",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
