using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LUMEN.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeCycles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_AspNetUsers_UserId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_UserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyTracks_AspNetUsers_UserId",
                table: "DailyTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Subjects_SubjectId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_StudyGroups_StudyGroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_DiscussionThreads_DiscussionThreadId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomMembers_AspNetUsers_UserId",
                table: "RoomMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomMembers_ChatRooms_ChatRoomId",
                table: "RoomMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_AspNetUsers_OwnerId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadges_AspNetUsers_UserId",
                table: "UserBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadges_Badges_BadgeId",
                table: "UserBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoProgresses_AspNetUsers_UserId",
                table: "VideoProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoProgresses_Lessons_LessonId",
                table: "VideoProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_XPLogs_AspNetUsers_UserId",
                table: "XPLogs");

            migrationBuilder.DropTable(
                name: "OnlineUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoProgresses",
                table: "VideoProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomMembers",
                table: "RoomMembers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "VideoProgresses",
                newName: "VideoProgress");

            migrationBuilder.RenameTable(
                name: "RoomMembers",
                newName: "RoomMember");

            migrationBuilder.RenameColumn(
                name: "DiscussionThreadId",
                table: "Posts",
                newName: "ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_DiscussionThreadId",
                table: "Posts",
                newName: "IX_Posts_ThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoProgresses_UserId",
                table: "VideoProgress",
                newName: "IX_VideoProgress_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoProgresses_LessonId",
                table: "VideoProgress",
                newName: "IX_VideoProgress_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomMembers_UserId",
                table: "RoomMember",
                newName: "IX_RoomMember_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomMembers_ChatRoomId",
                table: "RoomMember",
                newName: "IX_RoomMember_ChatRoomId");

            migrationBuilder.AddColumn<int>(
                name: "BadgeId1",
                table: "UserBadges",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Level",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Subjects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "StudyGroups",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StudyGroups",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StudyGroups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "StudyGroups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "StudyGroups",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuizId1",
                table: "QuizAttempts",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "GroupMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentLevel",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoProgress",
                table: "VideoProgress",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomMember",
                table: "RoomMember",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_BadgeId1",
                table: "UserBadges",
                column: "BadgeId1");

            migrationBuilder.CreateIndex(
                name: "IX_StudyGroups_CreatedBy",
                table: "StudyGroups",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId1",
                table: "QuizAttempts",
                column: "QuizId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_AspNetUsers_UserId",
                table: "Bookmarks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_UserId",
                table: "ChatMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTracks_AspNetUsers_UserId",
                table: "DailyTracks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Subjects_SubjectId",
                table: "Enrollments",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_StudyGroups_StudyGroupId",
                table: "GroupMembers",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_DiscussionThreads_ThreadId",
                table: "Posts",
                column: "ThreadId",
                principalTable: "DiscussionThreads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId1",
                table: "QuizAttempts",
                column: "QuizId1",
                principalTable: "Quizzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMember_AspNetUsers_UserId",
                table: "RoomMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMember_ChatRooms_ChatRoomId",
                table: "RoomMember",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_AspNetUsers_CreatedBy",
                table: "StudyGroups",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_AspNetUsers_OwnerId",
                table: "StudyGroups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadges_AspNetUsers_UserId",
                table: "UserBadges",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadges_Badges_BadgeId",
                table: "UserBadges",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadges_Badges_BadgeId1",
                table: "UserBadges",
                column: "BadgeId1",
                principalTable: "Badges",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideoProgress_AspNetUsers_UserId",
                table: "VideoProgress",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoProgress_Lessons_LessonId",
                table: "VideoProgress",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_XPLogs_AspNetUsers_UserId",
                table: "XPLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_AspNetUsers_UserId",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_UserId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyTracks_AspNetUsers_UserId",
                table: "DailyTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Subjects_SubjectId",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_StudyGroups_StudyGroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_DiscussionThreads_ThreadId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId1",
                table: "QuizAttempts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomMember_AspNetUsers_UserId",
                table: "RoomMember");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomMember_ChatRooms_ChatRoomId",
                table: "RoomMember");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_AspNetUsers_CreatedBy",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_AspNetUsers_OwnerId",
                table: "StudyGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadges_AspNetUsers_UserId",
                table: "UserBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadges_Badges_BadgeId",
                table: "UserBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBadges_Badges_BadgeId1",
                table: "UserBadges");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoProgress_AspNetUsers_UserId",
                table: "VideoProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoProgress_Lessons_LessonId",
                table: "VideoProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_XPLogs_AspNetUsers_UserId",
                table: "XPLogs");

            migrationBuilder.DropIndex(
                name: "IX_UserBadges_BadgeId1",
                table: "UserBadges");

            migrationBuilder.DropIndex(
                name: "IX_StudyGroups_CreatedBy",
                table: "StudyGroups");

            migrationBuilder.DropIndex(
                name: "IX_QuizAttempts_QuizId1",
                table: "QuizAttempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideoProgress",
                table: "VideoProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomMember",
                table: "RoomMember");

            migrationBuilder.DropColumn(
                name: "BadgeId1",
                table: "UserBadges");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudyGroups");

            migrationBuilder.DropColumn(
                name: "QuizId1",
                table: "QuizAttempts");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "VideoProgress",
                newName: "VideoProgresses");

            migrationBuilder.RenameTable(
                name: "RoomMember",
                newName: "RoomMembers");

            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "Posts",
                newName: "DiscussionThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ThreadId",
                table: "Posts",
                newName: "IX_Posts_DiscussionThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoProgress_UserId",
                table: "VideoProgresses",
                newName: "IX_VideoProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VideoProgress_LessonId",
                table: "VideoProgresses",
                newName: "IX_VideoProgresses_LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomMember_UserId",
                table: "RoomMembers",
                newName: "IX_RoomMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoomMember_ChatRoomId",
                table: "RoomMembers",
                newName: "IX_RoomMembers_ChatRoomId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Subjects",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Subjects",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Subjects",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "StudyGroups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "StudyGroups",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StudyGroups",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "GroupMembers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentLevel",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideoProgresses",
                table: "VideoProgresses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomMembers",
                table: "RoomMembers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OnlineUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnlineUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnlineUsers_UserId",
                table: "OnlineUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLogs_AspNetUsers_UserId",
                table: "AuditLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_AspNetUsers_UserId",
                table: "Bookmarks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_AspNetUsers_UserId",
                table: "Certificates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_UserId",
                table: "ChatMessages",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTracks_AspNetUsers_UserId",
                table: "DailyTracks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_AspNetUsers_UserId",
                table: "Enrollments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Subjects_SubjectId",
                table: "Enrollments",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_AspNetUsers_UserId",
                table: "Feedbacks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_UserId",
                table: "GroupMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_StudyGroups_StudyGroupId",
                table: "GroupMembers",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_DiscussionThreads_DiscussionThreadId",
                table: "Posts",
                column: "DiscussionThreadId",
                principalTable: "DiscussionThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_AspNetUsers_UserId",
                table: "QuizAttempts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempts_Quizzes_QuizId",
                table: "QuizAttempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMembers_AspNetUsers_UserId",
                table: "RoomMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMembers_ChatRooms_ChatRoomId",
                table: "RoomMembers",
                column: "ChatRoomId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_AspNetUsers_OwnerId",
                table: "StudyGroups",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadges_AspNetUsers_UserId",
                table: "UserBadges",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBadges_Badges_BadgeId",
                table: "UserBadges",
                column: "BadgeId",
                principalTable: "Badges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoProgresses_AspNetUsers_UserId",
                table: "VideoProgresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoProgresses_Lessons_LessonId",
                table: "VideoProgresses",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_XPLogs_AspNetUsers_UserId",
                table: "XPLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
