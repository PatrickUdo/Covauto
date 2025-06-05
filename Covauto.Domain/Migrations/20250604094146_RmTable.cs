using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Covauto.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RmTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeenAutoRitten");

            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "Autos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Naam",
                table: "Autos");

            migrationBuilder.CreateTable(
                name: "LeenAutoRitten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AutoId = table.Column<int>(type: "INTEGER", nullable: false),
                    WerknemerId = table.Column<string>(type: "TEXT", nullable: false),
                    AankomstTijd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    KilometerstandBegin = table.Column<int>(type: "INTEGER", nullable: false),
                    KilometerstandEind = table.Column<int>(type: "INTEGER", nullable: false),
                    NaarAdres = table.Column<string>(type: "TEXT", nullable: false),
                    VanAdres = table.Column<string>(type: "TEXT", nullable: false),
                    VertrekTijd = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeenAutoRitten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeenAutoRitten_AspNetUsers_WerknemerId",
                        column: x => x.WerknemerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeenAutoRitten_Autos_AutoId",
                        column: x => x.AutoId,
                        principalTable: "Autos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeenAutoRitten_AutoId",
                table: "LeenAutoRitten",
                column: "AutoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeenAutoRitten_WerknemerId",
                table: "LeenAutoRitten",
                column: "WerknemerId");
        }
    }
}
