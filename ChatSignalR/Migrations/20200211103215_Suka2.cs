using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatSignalR.Migrations
{
    public partial class Suka2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnLine",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnLine",
                table: "Users");
        }
    }
}
