using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Covauto.Domain.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeenAutoRitten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WerknemerId = table.Column<int>(type: "INTEGER", nullable: false),
                    VanAdres = table.Column<string>(type: "TEXT", nullable: false),
                    NaarAdres = table.Column<string>(type: "TEXT", nullable: false),
                    VertrekTijd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    KilometerstandBegin = table.Column<int>(type: "INTEGER", nullable: false),
                    KilometerstandEind = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeenAutoRitten", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeenAutoRitten");
        }
    }
}
