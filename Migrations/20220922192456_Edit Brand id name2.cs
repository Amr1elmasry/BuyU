using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class EditBrandidname2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Brands",
                newName: "BrandName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Brands",
                newName: "BrandId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "Brands",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "Brands",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
