using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradifyWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignmentSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubmissionUrl",
                table: "StudentHomeworkAssignments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmissionUrl",
                table: "StudentHomeworkAssignments");
        }
    }
}
