using Microsoft.EntityFrameworkCore.Migrations;

namespace Noter.DAL.Migrations
{
    public partial class RemovedColumnRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Commentaries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Commentaries",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
