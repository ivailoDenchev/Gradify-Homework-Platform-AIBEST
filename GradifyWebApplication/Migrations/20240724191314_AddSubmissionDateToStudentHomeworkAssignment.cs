using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GradifyWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionDateToStudentHomeworkAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubmissionUrl",
                table: "StudentHomeworkAssignments",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "StudentHomeworkAssignments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentHomeworkAssignments_StudentId",
                table: "StudentHomeworkAssignments",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentHomeworkAssignments_AspNetUsers_StudentId",
                table: "StudentHomeworkAssignments",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentHomeworkAssignments_AspNetUsers_StudentId",
                table: "StudentHomeworkAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentHomeworkAssignments_StudentId",
                table: "StudentHomeworkAssignments");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "StudentHomeworkAssignments");

            migrationBuilder.AlterColumn<string>(
                name: "SubmissionUrl",
                table: "StudentHomeworkAssignments",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
