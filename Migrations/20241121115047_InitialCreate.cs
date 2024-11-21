using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PRO05_BACK_Gaj_Khalos_Max.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "collections",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    theme_id = table.Column<string>(type: "text", nullable: false),
                    likes = table.Column<int>(type: "integer", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_collections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pokemon",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    api_url = table.Column<string>(type: "text", nullable: false),
                    likes = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pokemon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pokemon_collections",
                columns: table => new
                {
                    pokemon_id = table.Column<int>(type: "integer", nullable: false),
                    collection_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pokemon_collections", x => new { x.pokemon_id, x.collection_id });
                    table.ForeignKey(
                        name: "fk_pokemon_collections_collections_collection_id",
                        column: x => x.collection_id,
                        principalTable: "collections",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pokemon_collections_pokemon_pokemon_id",
                        column: x => x.pokemon_id,
                        principalTable: "pokemon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_pokemon_collections_collection_id",
                table: "pokemon_collections",
                column: "collection_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pokemon_collections");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "collections");

            migrationBuilder.DropTable(
                name: "pokemon");
        }
    }
}
