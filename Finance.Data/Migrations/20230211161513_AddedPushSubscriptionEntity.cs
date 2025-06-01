using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Data.Migrations
{
    public partial class AddedPushSubscriptionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PushSubscriptions",
                columns: table => new
                {
                    PushSubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserDeteilId = table.Column<int>(type: "int", nullable: false),
                    Endpoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EncriptionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Secret = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushSubscriptions", x => new { x.PushSubscriptionId, x.UserDeteilId });
                    table.ForeignKey(
                        name: "FK_PushSubscription_UserDetail",
                        column: x => x.UserDeteilId,
                        principalSchema: "dbo",
                        principalTable: "UserDetail",
                        principalColumn: "UserDetailId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PushSubscriptions_UserDeteilId",
                table: "PushSubscriptions",
                column: "UserDeteilId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PushSubscriptions");
        }
    }
}
