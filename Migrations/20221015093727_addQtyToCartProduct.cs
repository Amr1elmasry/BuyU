using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class addQtyToCartProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CartProduct",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CartProduct");
        }
    }
}
