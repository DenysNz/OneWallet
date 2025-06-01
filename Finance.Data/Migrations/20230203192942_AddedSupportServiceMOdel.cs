using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedSupportServiceMOdel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportRequest",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserDetailId = table.Column<int>(type: "int", nullable: false),
                    RequestName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RequestEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RequestQuestion = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportRequest", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_SupportRequest_UserDetail",
                        column: x => x.UserDetailId,
                        principalSchema: "dbo",
                        principalTable: "UserDetail",
                        principalColumn: "UserDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportRequest_UserDetailId",
                table: "SupportRequest",
                column: "UserDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportRequest");
        }
    }
}
