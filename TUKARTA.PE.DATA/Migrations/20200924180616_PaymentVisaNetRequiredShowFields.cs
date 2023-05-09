using Microsoft.EntityFrameworkCore.Migrations;

namespace TUKARTA.PE.DATA.Migrations
{
    public partial class PaymentVisaNetRequiredShowFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentCardBrand",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentCurrency",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMaskedCardNumber",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTraceNumber",
                table: "Purchases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionId",
                table: "Purchases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentCardBrand",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentCurrency",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentMaskedCardNumber",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentTraceNumber",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionId",
                table: "Purchases");
        }
    }
}
