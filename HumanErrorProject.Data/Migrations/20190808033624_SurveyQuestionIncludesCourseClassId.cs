using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class SurveyQuestionIncludesCourseClassId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestion_CourseClasses_CourseClassId",
                table: "SurveyQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "CourseClassId",
                table: "SurveyQuestion",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestion_CourseClasses_CourseClassId",
                table: "SurveyQuestion",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyQuestion_CourseClasses_CourseClassId",
                table: "SurveyQuestion");

            migrationBuilder.AlterColumn<int>(
                name: "CourseClassId",
                table: "SurveyQuestion",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyQuestion_CourseClasses_CourseClassId",
                table: "SurveyQuestion",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
