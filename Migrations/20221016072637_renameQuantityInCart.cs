using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class renameQuantityInCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quntity",
                table: "Carts",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Carts",
                newName: "Quntity");
        }
    }
}
