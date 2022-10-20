using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class EditOrderTableREQUIRED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 19, 18, 49, 37, 576, DateTimeKind.Local).AddTicks(8320),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 19, 18, 43, 38, 555, DateTimeKind.Local).AddTicks(8720));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Orders",
                type: "nvarchar(265)",
                maxLength: 265,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(265)",
                oldMaxLength: 265);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 19, 18, 43, 38, 555, DateTimeKind.Local).AddTicks(8720),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 19, 18, 49, 37, 576, DateTimeKind.Local).AddTicks(8320));
        }
    }
}
