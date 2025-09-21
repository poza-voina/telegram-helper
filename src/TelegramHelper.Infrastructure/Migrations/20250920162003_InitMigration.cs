using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TelegramHelper.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "folder",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: true),
                    owner_id = table.Column<long>(type: "bigint", nullable: false),
                    folder_id = table.Column<int>(type: "integer", nullable: false),
                    icon_name = table.Column<string>(type: "text", nullable: false),
                    folder_name = table.Column<string>(type: "text", nullable: false),
                    folder_type = table.Column<int>(type: "integer", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "timezone('utc', now())"),
                    update_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folder", x => x.id);
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
                    FolderModelId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_folder", x => x.id);
                    table.ForeignKey(
                        name: "FK_chat_folder_folder_FolderModelId",
                        column: x => x.FolderModelId,
                        principalTable: "folder",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_chat_folder_FolderModelId",
                table: "chat_folder",
                column: "FolderModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_folder");

            migrationBuilder.DropTable(
                name: "folder");
        }
    }
}
