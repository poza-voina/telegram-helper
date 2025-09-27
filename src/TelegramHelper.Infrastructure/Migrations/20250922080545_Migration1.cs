using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TelegramHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "owner",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owner", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "folder",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_id = table.Column<long>(type: "bigint", nullable: false),
                    folder_id = table.Column<int>(type: "integer", nullable: false),
                    icon_name = table.Column<string>(type: "text", nullable: false),
                    folder_name = table.Column<string>(type: "text", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folder", x => x.id);
                    table.ForeignKey(
                        name: "FK_folder_owner_owner_id",
                        column: x => x.owner_id,
                        principalTable: "owner",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_folder",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    folder_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    CurrentFolderModelId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_folder", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_folder_folder_CurrentFolderModelId",
                        column: x => x.CurrentFolderModelId,
                        principalTable: "folder",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "folder_filters",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    folder_id = table.Column<long>(type: "bigint", nullable: false),
                    filter_type = table.Column<int>(type: "integer", nullable: false),
                    CurrentFolderModelId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folder_filters", x => x.id);
                    table.ForeignKey(
                        name: "FK_folder_filters_folder_CurrentFolderModelId",
                        column: x => x.CurrentFolderModelId,
                        principalTable: "folder",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_folder_CurrentFolderModelId",
                table: "chat_folder",
                column: "CurrentFolderModelId");

            migrationBuilder.CreateIndex(
                name: "IX_folder_owner_id_folder_id",
                table: "folder",
                columns: new[] { "owner_id", "folder_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_folder_filters_CurrentFolderModelId",
                table: "folder_filters",
                column: "CurrentFolderModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_folder");

            migrationBuilder.DropTable(
                name: "folder_filters");

            migrationBuilder.DropTable(
                name: "folder");

            migrationBuilder.DropTable(
                name: "owner");
        }
    }
}
