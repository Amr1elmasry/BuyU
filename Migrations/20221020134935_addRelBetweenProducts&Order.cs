using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations
{
    public partial class addRelBetweenProductsOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 20, 15, 49, 35, 781, DateTimeKind.Local).AddTicks(2929),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 20, 15, 15, 4, 28, DateTimeKind.Local).AddTicks(3241));

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    OrdersOrderId = table.Column<int>(type: "int", nullable: false),
                    productsProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => new { x.OrdersOrderId, x.productsProductId });
                    table.ForeignKey(
                        name: "FK_OrderProduct_Orders_OrdersOrderId",
                        column: x => x.OrdersOrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Products_productsProductId",
                        column: x => x.productsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_productsProductId",
                table: "OrderProduct",
                column: "productsProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.AlterColumn<DateTime>(
                name: "dateTime",
                table: "CartProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 10, 20, 15, 15, 4, 28, DateTimeKind.Local).AddTicks(3241),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 10, 20, 15, 49, 35, 781, DateTimeKind.Local).AddTicks(2929));
        }
    }
}
