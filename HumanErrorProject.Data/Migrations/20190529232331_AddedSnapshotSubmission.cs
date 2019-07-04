using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HumanErrorProject.Data.Migrations
{
    public partial class AddedSnapshotSubmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Files",
                table: "Snapshots");

            migrationBuilder.AddColumn<int>(
                name: "SnapshotSubmissionId",
                table: "Snapshots",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SnapshotSubmission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Files = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SnapshotSubmission", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SnapshotSubmissionId",
                table: "Snapshots",
                column: "SnapshotSubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snapshots_SnapshotSubmission_SnapshotSubmissionId",
                table: "Snapshots",
                column: "SnapshotSubmissionId",
                principalTable: "SnapshotSubmission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snapshots_SnapshotSubmission_SnapshotSubmissionId",
                table: "Snapshots");

            migrationBuilder.DropTable(
                name: "SnapshotSubmission");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_SnapshotSubmissionId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "SnapshotSubmissionId",
                table: "Snapshots");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Snapshots",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "Files",
                table: "Snapshots",
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
