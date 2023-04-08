using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMaker.Data.Migrations
{
    public partial class Survey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survey", x => x.SurveyId);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswerIndex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Question_Survey_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Survey",
                        principalColumn: "SurveyId");
                });

            migrationBuilder.CreateTable(
                name: "QuestionChoice",
                columns: table => new
                {
                    QuestionChoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_Question_SurveyId",
                table: "Question",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionChoice_QuestionId",
                table: "QuestionChoice",
                column: "QuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionChoice");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Survey");
        }
    }
}
