using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class AddedPreAssignmentReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreAssignmentFailTestsFailureReportId",
                table: "UnitTests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PreAssignmentReport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<int>(nullable: false),
                    Report = table.Column<string>(nullable: true),
                    PreAssignmentCompileFailureReport_Report = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreAssignmentReport", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitTests_PreAssignmentFailTestsFailureReportId",
                table: "UnitTests",
                column: "PreAssignmentFailTestsFailureReportId");

            migrationBuilder.CreateIndex(
                name: "IX_MethodDeclarations_PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations",
                column: "PreAssignmentMissingMethodsFailureReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_MethodDeclarations_PreAssignmentReport_PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations",
                column: "PreAssignmentMissingMethodsFailureReportId",
                principalTable: "PreAssignmentReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTests_PreAssignmentReport_PreAssignmentFailTestsFailureReportId",
                table: "UnitTests",
                column: "PreAssignmentFailTestsFailureReportId",
                principalTable: "PreAssignmentReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MethodDeclarations_PreAssignmentReport_PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitTests_PreAssignmentReport_PreAssignmentFailTestsFailureReportId",
                table: "UnitTests");

            migrationBuilder.DropTable(
                name: "PreAssignmentReport");

            migrationBuilder.DropIndex(
                name: "IX_UnitTests_PreAssignmentFailTestsFailureReportId",
                table: "UnitTests");

            migrationBuilder.DropIndex(
                name: "IX_MethodDeclarations_PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations");

            migrationBuilder.DropColumn(
                name: "PreAssignmentFailTestsFailureReportId",
                table: "UnitTests");

            migrationBuilder.DropColumn(
                name: "PreAssignmentMissingMethodsFailureReportId",
                table: "MethodDeclarations");
        }
    }
}
