using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class CourceClassWithQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseClassId",
                table: "SurveyQuestion",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_CourseClassId",
                table: "SurveyQuestion",
                column: "CourseClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestion_CourseClasses_CourseClassId",
                table: "SurveyQuestion",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestion_CourseClasses_CourseClassId",
                table: "SurveyQuestion");

            migrationBuilder.DropIndex(
                name: "IX_SurveyQuestion_CourseClassId",
                table: "SurveyQuestion");

            migrationBuilder.DropColumn(
                name: "CourseClassId",
                table: "SurveyQuestion");
        }
    }
}
