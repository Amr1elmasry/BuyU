using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class addOrdersInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 20, 15, 15, 4, 28, DateTimeKind.Local).AddTicks(3241),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 19, 18, 49, 37, 576, DateTimeKind.Local).AddTicks(8320));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 19, 18, 49, 37, 576, DateTimeKind.Local).AddTicks(8320),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 20, 15, 15, 4, 28, DateTimeKind.Local).AddTicks(3241));
        }
    }
}
