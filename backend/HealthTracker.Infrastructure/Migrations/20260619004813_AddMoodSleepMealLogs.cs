using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoodSleepMealLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Moodlogs_Users_UserId",
                table: "Moodlogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SleepLog_Users_UserId",
                table: "SleepLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Moodlogs",
                table: "Moodlogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SleepLog",
                table: "SleepLog");

            migrationBuilder.RenameTable(
                name: "Moodlogs",
                newName: "MoodLogs");

            migrationBuilder.RenameTable(
                name: "SleepLog",
                newName: "SleepLogs");

            migrationBuilder.RenameIndex(
                name: "IX_Moodlogs_UserId",
                table: "MoodLogs",
                newName: "IX_MoodLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SleepLog_UserId",
                table: "SleepLogs",
                newName: "IX_SleepLogs_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoodLogs",
                table: "MoodLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SleepLogs",
                table: "SleepLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoodLogs_Users_UserId",
                table: "MoodLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SleepLogs_Users_UserId",
                table: "SleepLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoodLogs_Users_UserId",
                table: "MoodLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SleepLogs_Users_UserId",
                table: "SleepLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MoodLogs",
                table: "MoodLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SleepLogs",
                table: "SleepLogs");

            migrationBuilder.RenameTable(
                name: "MoodLogs",
                newName: "Moodlogs");

            migrationBuilder.RenameTable(
                name: "SleepLogs",
                newName: "SleepLog");

            migrationBuilder.RenameIndex(
                name: "IX_MoodLogs_UserId",
                table: "Moodlogs",
                newName: "IX_Moodlogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SleepLogs_UserId",
                table: "SleepLog",
                newName: "IX_SleepLog_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Moodlogs",
                table: "Moodlogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SleepLog",
                table: "SleepLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Moodlogs_Users_UserId",
                table: "Moodlogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SleepLog_Users_UserId",
                table: "SleepLog",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
