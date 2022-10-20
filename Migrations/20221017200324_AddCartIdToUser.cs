using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class AddCartIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 17, 22, 3, 24, 759, DateTimeKind.Local).AddTicks(4671),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 17, 20, 12, 54, 238, DateTimeKind.Local).AddTicks(5064));

            migrationBuilder.AddColumn<int>(
                name: "CartId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_UserId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 17, 20, 12, 54, 238, DateTimeKind.Local).AddTicks(5064),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 17, 22, 3, 24, 759, DateTimeKind.Local).AddTicks(4671));

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");
        }
    }
}
