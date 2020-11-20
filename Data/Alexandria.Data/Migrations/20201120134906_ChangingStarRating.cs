using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Alexandria.Data.Migrations
{
    public partial class ChangingStarRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "StarRatings");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StarRatings");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "StarRatings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "StarRatings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "StarRatings");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "StarRatings");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "StarRatings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StarRatings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
