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
                    telegram_folder_id = table.Column<int>(type: "integer", nullable: false),
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
                name: "current_dynamic_folder_filters",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    folder_id = table.Column<long>(type: "bigint", nullable: false),
                    filter_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current_dynamic_folder_filters", x => x.id);
                    table.ForeignKey(
                        name: "FK_current_dynamic_folder_filters_folder_folder_id",
                        column: x => x.folder_id,
                        principalTable: "folder",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "current_static_folder_filters",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    folder_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_current_static_folder_filters", x => x.id);
                    table.ForeignKey(
                        name: "FK_current_static_folder_filters_folder_folder_id",
                        column: x => x.folder_id,
                        principalTable: "folder",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_current_dynamic_folder_filters_folder_id",
                table: "current_dynamic_folder_filters",
                column: "folder_id");

            migrationBuilder.CreateIndex(
                name: "IX_current_static_folder_filters_folder_id",
                table: "current_static_folder_filters",
                column: "folder_id");

            migrationBuilder.CreateIndex(
                name: "IX_folder_owner_id_telegram_folder_id",
                table: "folder",
                columns: new[] { "owner_id", "telegram_folder_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "current_dynamic_folder_filters");

            migrationBuilder.DropTable(
                name: "current_static_folder_filters");

            migrationBuilder.DropTable(
                name: "folder");

            migrationBuilder.DropTable(
                name: "owner");
        }
    }
}
