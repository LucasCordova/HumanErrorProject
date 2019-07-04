using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class RenameTestProjectFolderToTestProjectFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestProjectFolder",
                table: "TestProjects",
                newName: "TestProjectFile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestProjectFile",
                table: "TestProjects",
                newName: "TestProjectFolder");
        }
    }
}
