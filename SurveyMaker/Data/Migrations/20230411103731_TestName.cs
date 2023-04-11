using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMaker.Data.Migrations
{
    public partial class TestName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TestName",
                table: "TestResults",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestName",
                table: "TestResults");
        }
    }
}
