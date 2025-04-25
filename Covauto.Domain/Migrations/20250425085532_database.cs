using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Covauto.Domain.Migrations
{
    /// <inheritdoc />
    public partial class database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LeenAutoRitten",
                table: "LeenAutoRitten");

            migrationBuilder.RenameTable(
                name: "LeenAutoRitten",
                newName: "LeenAutoRit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeenAutoRit",
                table: "LeenAutoRit",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LeenAutoRit",
                table: "LeenAutoRit");

            migrationBuilder.RenameTable(
                name: "LeenAutoRit",
                newName: "LeenAutoRitten");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeenAutoRitten",
                table: "LeenAutoRitten",
                column: "Id");
        }
    }
}
