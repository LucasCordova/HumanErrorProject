using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class DbContextHasPreAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignment_AssignmentSolutions_AssignmentSolutionId",
                table: "PreAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignment_CourseClasses_CourseClassId",
                table: "PreAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignment_PreAssignmentReport_PreAssignmentReportId",
                table: "PreAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignment_TestProjects_TestProjectId",
                table: "PreAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreAssignment",
                table: "PreAssignment");

            migrationBuilder.RenameTable(
                name: "PreAssignment",
                newName: "PreAssignments");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignment_TestProjectId",
                table: "PreAssignments",
                newName: "IX_PreAssignments_TestProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignment_PreAssignmentReportId",
                table: "PreAssignments",
                newName: "IX_PreAssignments_PreAssignmentReportId");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignment_CourseClassId",
                table: "PreAssignments",
                newName: "IX_PreAssignments_CourseClassId");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignment_AssignmentSolutionId",
                table: "PreAssignments",
                newName: "IX_PreAssignments_AssignmentSolutionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreAssignments",
                table: "PreAssignments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignments_AssignmentSolutions_AssignmentSolutionId",
                table: "PreAssignments",
                column: "AssignmentSolutionId",
                principalTable: "AssignmentSolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignments_CourseClasses_CourseClassId",
                table: "PreAssignments",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignments_PreAssignmentReport_PreAssignmentReportId",
                table: "PreAssignments",
                column: "PreAssignmentReportId",
                principalTable: "PreAssignmentReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignments_TestProjects_TestProjectId",
                table: "PreAssignments",
                column: "TestProjectId",
                principalTable: "TestProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignments_AssignmentSolutions_AssignmentSolutionId",
                table: "PreAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignments_CourseClasses_CourseClassId",
                table: "PreAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignments_PreAssignmentReport_PreAssignmentReportId",
                table: "PreAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_PreAssignments_TestProjects_TestProjectId",
                table: "PreAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PreAssignments",
                table: "PreAssignments");

            migrationBuilder.RenameTable(
                name: "PreAssignments",
                newName: "PreAssignment");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignments_TestProjectId",
                table: "PreAssignment",
                newName: "IX_PreAssignment_TestProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignments_PreAssignmentReportId",
                table: "PreAssignment",
                newName: "IX_PreAssignment_PreAssignmentReportId");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignments_CourseClassId",
                table: "PreAssignment",
                newName: "IX_PreAssignment_CourseClassId");

            migrationBuilder.RenameIndex(
                name: "IX_PreAssignments_AssignmentSolutionId",
                table: "PreAssignment",
                newName: "IX_PreAssignment_AssignmentSolutionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PreAssignment",
                table: "PreAssignment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignment_AssignmentSolutions_AssignmentSolutionId",
                table: "PreAssignment",
                column: "AssignmentSolutionId",
                principalTable: "AssignmentSolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignment_CourseClasses_CourseClassId",
                table: "PreAssignment",
                column: "CourseClassId",
                principalTable: "CourseClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignment_PreAssignmentReport_PreAssignmentReportId",
                table: "PreAssignment",
                column: "PreAssignmentReportId",
                principalTable: "PreAssignmentReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PreAssignment_TestProjects_TestProjectId",
                table: "PreAssignment",
                column: "TestProjectId",
                principalTable: "TestProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
