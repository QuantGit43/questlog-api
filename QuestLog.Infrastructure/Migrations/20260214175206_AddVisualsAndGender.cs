using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestLog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVisualsAndGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "FemaleAssetId",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaleAssetId",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecommendedClass",
                table: "Items",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisualGender",
                table: "Avatars",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FemaleAssetId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "MaleAssetId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RecommendedClass",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "VisualGender",
                table: "Avatars");

            migrationBuilder.AddColumn<string>(
                name: "AssetId",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
