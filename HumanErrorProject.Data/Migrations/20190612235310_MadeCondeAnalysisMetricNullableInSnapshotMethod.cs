using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class MadeCondeAnalysisMetricNullableInSnapshotMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotMethods_CodeAnalysisMetrics_CodeAnalysisMetricId",
                table: "SnapshotMethods");

            migrationBuilder.AlterColumn<int>(
                name: "CodeAnalysisMetricId",
                table: "SnapshotMethods",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotMethods_CodeAnalysisMetrics_CodeAnalysisMetricId",
                table: "SnapshotMethods",
                column: "CodeAnalysisMetricId",
                principalTable: "CodeAnalysisMetrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnapshotMethods_CodeAnalysisMetrics_CodeAnalysisMetricId",
                table: "SnapshotMethods");

            migrationBuilder.AlterColumn<int>(
                name: "CodeAnalysisMetricId",
                table: "SnapshotMethods",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SnapshotMethods_CodeAnalysisMetrics_CodeAnalysisMetricId",
                table: "SnapshotMethods",
                column: "CodeAnalysisMetricId",
                principalTable: "CodeAnalysisMetrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
