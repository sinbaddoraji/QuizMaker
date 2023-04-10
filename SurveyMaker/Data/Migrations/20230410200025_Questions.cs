using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMaker.Data.Migrations
{
    public partial class Questions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionChoice");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Test");

            migrationBuilder.AddColumn<string>(
                name: "Questions",
                table: "Test",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Questions",
                table: "Test");

            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Test",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswerIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Question_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "TestId");
                });

            migrationBuilder.CreateTable(
                name: "QuestionChoice",
                columns: table => new
                {
                    QuestionChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionChoice", x => x.QuestionChoiceId);
                    table.ForeignKey(
                        name: "FK_QuestionChoice_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_TestId",
                table: "Question",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionChoice_QuestionId",
                table: "QuestionChoice",
                column: "QuestionId");
        }
    }
}
