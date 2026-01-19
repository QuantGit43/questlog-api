using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestLog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDifficultyColumnToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Avatars_OwnerAvatarId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "OwnerAvatarId",
                table: "Tasks",
                newName: "AvatarId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_OwnerAvatarId",
                table: "Tasks",
                newName: "IX_Tasks_AvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Avatars_AvatarId",
                table: "Tasks",
                column: "AvatarId",
                principalTable: "Avatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Avatars_AvatarId",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "AvatarId",
                table: "Tasks",
                newName: "OwnerAvatarId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AvatarId",
                table: "Tasks",
                newName: "IX_Tasks_OwnerAvatarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Avatars_OwnerAvatarId",
                table: "Tasks",
                column: "OwnerAvatarId",
                principalTable: "Avatars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
