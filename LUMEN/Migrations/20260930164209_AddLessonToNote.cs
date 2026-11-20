using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LUMEN.Migrations
{
    /// <inheritdoc />
    public partial class AddLessonToNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Lessons_LessonId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "XPAmount",
                table: "XPLogs",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "SessionData",
                table: "VirtualLabSessions",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "EndedAt",
                table: "VirtualLabSessions",
                newName: "CompletedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "XPLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "XPLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "LabType",
                table: "VirtualLabSessions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Notes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_SubjectId",
                table: "Notes",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Lessons_LessonId",
                table: "Notes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Subjects_SubjectId",
                table: "Notes",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Lessons_LessonId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Subjects_SubjectId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_SubjectId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "XPLogs");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "XPLogs",
                newName: "XPAmount");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "VirtualLabSessions",
                newName: "EndedAt");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "VirtualLabSessions",
                newName: "SessionData");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "XPLogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LabType",
                table: "VirtualLabSessions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LessonId",
                table: "Notes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Lessons_LessonId",
                table: "Notes",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
