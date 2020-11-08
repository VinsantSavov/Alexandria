namespace Alexandria.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ReviewEditionRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ThisEdition",
                table: "Reviews",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ThisEdition",
                table: "Reviews",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
