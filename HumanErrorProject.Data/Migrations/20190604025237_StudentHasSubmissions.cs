using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class StudentHasSubmissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "SnapshotSubmission",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SnapshotSubmission_StudentId",
                table: "SnapshotSubmission",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotSubmission_Students_StudentId",
                table: "SnapshotSubmission",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotSubmission_Students_StudentId",
                table: "SnapshotSubmission");

            migrationBuilder.DropIndex(
                name: "IX_SnapshotSubmission_StudentId",
                table: "SnapshotSubmission");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "SnapshotSubmission");
        }
    }
}
