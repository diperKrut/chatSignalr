using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatSignalR.Migrations
{
    public partial class Mapper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Massages_UserMapper_UserId",
                table: "Massages");

            migrationBuilder.DropForeignKey(
                name: "FK_Massages_Users_UserId1",
                table: "Massages");

            migrationBuilder.DropTable(
                name: "UserMapper");

            migrationBuilder.DropIndex(
                name: "IX_Massages_UserId1",
                table: "Massages");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Massages");

            migrationBuilder.AddForeignKey(
                name: "FK_Massages_Users_UserId",
                table: "Massages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Massages_Users_UserId",
                table: "Massages");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Massages",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserMapper",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMapper", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Massages_UserId1",
                table: "Massages",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Massages_UserMapper_UserId",
                table: "Massages",
                column: "UserId",
                principalTable: "UserMapper",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Massages_Users_UserId1",
                table: "Massages",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
