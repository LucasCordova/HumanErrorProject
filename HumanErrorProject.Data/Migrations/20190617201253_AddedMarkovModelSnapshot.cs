using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class AddedMarkovModelSnapshot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_MarkovModelStates_MarkovModelStateId",
                table: "Snapshots");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_MarkovModelStateId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "MarkovModelStateId",
                table: "Snapshots");

            migrationBuilder.CreateTable(
                name: "MarkovModelSnapshot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SnapshotId = table.Column<int>(nullable: false),
                    MarkovModelStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarkovModelSnapshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarkovModelSnapshot_MarkovModelStates_MarkovModelStateId",
                        column: x => x.MarkovModelStateId,
                        principalTable: "MarkovModelStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarkovModelSnapshot_MarkovModelStateId",
                table: "MarkovModelSnapshot",
                column: "MarkovModelStateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarkovModelSnapshot");

            migrationBuilder.AddColumn<int>(
                name: "MarkovModelStateId",
                table: "Snapshots",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_MarkovModelStateId",
                table: "Snapshots",
                column: "MarkovModelStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_MarkovModelStates_MarkovModelStateId",
                table: "Snapshots",
                column: "MarkovModelStateId",
                principalTable: "MarkovModelStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
