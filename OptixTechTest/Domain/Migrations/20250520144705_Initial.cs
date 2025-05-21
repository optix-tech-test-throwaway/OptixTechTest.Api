using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptixTechTest.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Overview = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    Popularity = table.Column<decimal>(type: "numeric", nullable: false),
                    VoteCount = table.Column<long>(type: "bigint", nullable: false),
                    VoteAverage = table.Column<decimal>(type: "numeric", nullable: false),
                    OriginalLanguage = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Genres = table.Column<List<string>>(type: "text[]", nullable: false),
                    Actors = table.Column<List<string>>(type: "text[]", nullable: false),
                    PosterUrl = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Actors",
                table: "Movies",
                column: "Actors")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Genres",
                table: "Movies",
                column: "Genres")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Popularity",
                table: "Movies",
                column: "Popularity");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ReleaseDate",
                table: "Movies",
                column: "ReleaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Title",
                table: "Movies",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_VoteAverage",
                table: "Movies",
                column: "VoteAverage");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_VoteCount",
                table: "Movies",
                column: "VoteCount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
