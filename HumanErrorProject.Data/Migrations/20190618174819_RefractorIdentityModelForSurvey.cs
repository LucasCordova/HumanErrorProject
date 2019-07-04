using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class RefractorIdentityModelForSurvey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Surveys_SurveyKey",
                table: "Snapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyKey",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_SurveyKey",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "SurveyKey",
                table: "Snapshots");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "Surveys",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SurveyKey",
                table: "SurveyResponses",
                newName: "SurveyId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResponses_SurveyKey",
                table: "SurveyResponses",
                newName: "IX_SurveyResponses_SurveyId");

            migrationBuilder.AlterColumn<string>(
                name: "SurveyId",
                table: "Snapshots",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SurveyId",
                table: "Snapshots",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_Surveys_SurveyId",
                table: "Snapshots",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyId",
                table: "SurveyResponses",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Surveys_SurveyId",
                table: "Snapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_SurveyId",
                table: "Snapshots");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Surveys",
                newName: "Key");

            migrationBuilder.RenameColumn(
                name: "SurveyId",
                table: "SurveyResponses",
                newName: "SurveyKey");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResponses_SurveyId",
                table: "SurveyResponses",
                newName: "IX_SurveyResponses_SurveyKey");

            migrationBuilder.AlterColumn<int>(
                name: "SurveyId",
                table: "Snapshots",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurveyKey",
                table: "Snapshots",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SurveyKey",
                table: "Snapshots",
                column: "SurveyKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_Surveys_SurveyKey",
                table: "Snapshots",
                column: "SurveyKey",
                principalTable: "Surveys",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyKey",
                table: "SurveyResponses",
                column: "SurveyKey",
                principalTable: "Surveys",
                principalColumn: "Key",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
