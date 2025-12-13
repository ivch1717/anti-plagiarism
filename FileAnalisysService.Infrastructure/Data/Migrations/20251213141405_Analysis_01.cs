using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileAnalisysService.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Analysis_01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlagiarismReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsPlagiarism = table.Column<bool>(type: "boolean", nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    OriginalWorkId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlagiarismReports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlagiarismReports_Status",
                table: "PlagiarismReports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PlagiarismReports_WorkId",
                table: "PlagiarismReports",
                column: "WorkId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Works_AssignmentId",
                table: "Works",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_AssignmentId_SubmittedAt",
                table: "Works",
                columns: new[] { "AssignmentId", "SubmittedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlagiarismReports");

            migrationBuilder.DropTable(
                name: "Works");
        }
    }
}
