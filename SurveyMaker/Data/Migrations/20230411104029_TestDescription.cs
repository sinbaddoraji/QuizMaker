using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMaker.Data.Migrations
{
    public partial class TestDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestDescription",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestDescription",
                table: "TestResults");
        }
    }
}
