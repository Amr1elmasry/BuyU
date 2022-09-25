using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations.BuyUIdentity
{
    public partial class AddAdminUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");


            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Country], [Image], [CartId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'5f8cb6a5-420b-44f4-ab30-411a97955647', N'Amr', N'Rizk', NULL, NULL, NULL, N'Amr.rizk733', N'AMR.RIZK733', N'Amr.rizk733@gmail.com', N'AMR.RIZK733@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEF3JGnJ7IHaK0EAgLmN2vgScynwIjoYlOrvR0Defk5nu7+95Z61ZQF4z+yT6aKP1dg==', N'UB4CXQO5K76HIXJPEZ7XULIKUGW7X4FP', N'3da1b694-cfb4-4cbc-80dc-20f499ad0889', N'01151059974', 0, 0, NULL, 1, 0)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = '5f8cb6a5-420b-44f4-ab30-411a97955647' ");

        }
    }
}
