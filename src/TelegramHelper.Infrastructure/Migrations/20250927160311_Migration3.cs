using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_current_dynamic_folder_filters_folder_folder_id",
                table: "current_dynamic_folder_filters");

            migrationBuilder.DropForeignKey(
                name: "FK_current_static_folder_filters_folder_folder_id",
                table: "current_static_folder_filters");

            migrationBuilder.DropForeignKey(
                name: "FK_folder_owner_owner_id",
                table: "folder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_folder",
                table: "folder");

            migrationBuilder.RenameTable(
                name: "folder",
                newName: "current_folders");

            migrationBuilder.RenameIndex(
                name: "IX_folder_owner_id_telegram_folder_id",
                table: "current_folders",
                newName: "IX_current_folders_owner_id_telegram_folder_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_current_folders",
                table: "current_folders",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_current_dynamic_folder_filters_current_folders_folder_id",
                table: "current_dynamic_folder_filters",
                column: "folder_id",
                principalTable: "current_folders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_current_folders_owner_owner_id",
                table: "current_folders",
                column: "owner_id",
                principalTable: "owner",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_current_static_folder_filters_current_folders_folder_id",
                table: "current_static_folder_filters",
                column: "folder_id",
                principalTable: "current_folders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_current_dynamic_folder_filters_current_folders_folder_id",
                table: "current_dynamic_folder_filters");

            migrationBuilder.DropForeignKey(
                name: "FK_current_folders_owner_owner_id",
                table: "current_folders");

            migrationBuilder.DropForeignKey(
                name: "FK_current_static_folder_filters_current_folders_folder_id",
                table: "current_static_folder_filters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_current_folders",
                table: "current_folders");

            migrationBuilder.RenameTable(
                name: "current_folders",
                newName: "folder");

            migrationBuilder.RenameIndex(
                name: "IX_current_folders_owner_id_telegram_folder_id",
                table: "folder",
                newName: "IX_folder_owner_id_telegram_folder_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_folder",
                table: "folder",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_current_dynamic_folder_filters_folder_folder_id",
                table: "current_dynamic_folder_filters",
                column: "folder_id",
                principalTable: "folder",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_current_static_folder_filters_folder_folder_id",
                table: "current_static_folder_filters",
                column: "folder_id",
                principalTable: "folder",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_folder_owner_owner_id",
                table: "folder",
                column: "owner_id",
                principalTable: "owner",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
