using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class ClassHasPreAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Filename = table.Column<string>(maxLength: 256, nullable: false),
                    CourseClassId = table.Column<int>(nullable: false),
                    AssignmentSolutionId = table.Column<int>(nullable: false),
                    TestProjectId = table.Column<int>(nullable: false),
                    PreAssignmentReportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreAssignment_AssignmentSolutions_AssignmentSolutionId",
                        column: x => x.AssignmentSolutionId,
                        principalTable: "AssignmentSolutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAssignment_CourseClasses_CourseClassId",
                        column: x => x.CourseClassId,
                        principalTable: "CourseClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAssignment_PreAssignmentReport_PreAssignmentReportId",
                        column: x => x.PreAssignmentReportId,
                        principalTable: "PreAssignmentReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreAssignment_TestProjects_TestProjectId",
                        column: x => x.TestProjectId,
                        principalTable: "TestProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignment_AssignmentSolutionId",
                table: "PreAssignment",
                column: "AssignmentSolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignment_CourseClassId",
                table: "PreAssignment",
                column: "CourseClassId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignment_PreAssignmentReportId",
                table: "PreAssignment",
                column: "PreAssignmentReportId");

            migrationBuilder.CreateIndex(
                name: "IX_PreAssignment_TestProjectId",
                table: "PreAssignment",
                column: "TestProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreAssignment");
        }
    }
}
