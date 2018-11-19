using Microsoft.EntityFrameworkCore.Migrations;

namespace Noter.DAL.Migrations
{
    public partial class AddedColumnIsPageToTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPage",
                table: "Topics",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPage",
                table: "Topics");
        }
    }
}
