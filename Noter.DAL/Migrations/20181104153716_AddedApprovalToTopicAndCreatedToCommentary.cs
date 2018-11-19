using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Noter.DAL.Migrations
{
    public partial class AddedApprovalToTopicAndCreatedToCommentary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Approval",
                table: "Topics",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Commentaries",
                nullable: false,
                defaultValue: new DateTime(2018, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approval",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Commentaries");
        }
    }
}
