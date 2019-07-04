using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class SurveyStandsOnItOwn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Surveys_SurveyId",
                table: "Snapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyId",
                table: "SurveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_SurveyId",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_SurveyId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "SurveyId",
                table: "SurveyResponses");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Surveys",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "SurveyKey",
                table: "SurveyResponses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurveyKey",
                table: "Snapshots",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyKey",
                table: "SurveyResponses",
                column: "SurveyKey");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_Surveys_SurveyKey",
                table: "Snapshots");

            migrationBuilder.DropForeignKey(
                name: "FK_SurveyResponses_Surveys_SurveyKey",
                table: "SurveyResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_SurveyKey",
                table: "SurveyResponses");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_SurveyKey",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "SurveyKey",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "SurveyKey",
                table: "Snapshots");

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "Surveys",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Surveys",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "SurveyId",
                table: "SurveyResponses",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Surveys",
                table: "Surveys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SurveyId",
                table: "SurveyResponses",
                column: "SurveyId");

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
    }
}
