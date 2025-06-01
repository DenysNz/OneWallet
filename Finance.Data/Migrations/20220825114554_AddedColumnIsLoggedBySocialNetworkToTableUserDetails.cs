using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedColumnIsLoggedBySocialNetworkToTableUserDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLoggedBySocialNetwork",
                schema: "dbo",
                table: "UserDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLoggedBySocialNetwork",
                schema: "dbo",
                table: "UserDetail");
        }
    }
}
