using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuyU.Migrations.BuyUIdentity
{
    public partial class assignAdminUserToAllRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUserRoles] (UserId , RoleId) SELECT '5f8cb6a5-420b-44f4-ab30-411a97955647' , Id FROM [dbo].[AspNetRoles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUserRoles] WHERE UserId = '5f8cb6a5-420b-44f4-ab30-411a97955647'");
        }
    }
}
