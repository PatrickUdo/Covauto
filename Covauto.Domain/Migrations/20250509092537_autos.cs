using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Covauto.Domain.Migrations
{
    /// <inheritdoc />
    public partial class autos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LeenAutoRit",
                table: "LeenAutoRit");

            migrationBuilder.RenameTable(
                name: "LeenAutoRit",
                newName: "LeenAutoRitten");

            migrationBuilder.AlterColumn<string>(
                name: "WerknemerId",
                table: "LeenAutoRitten",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "AankomstTijd",
                table: "LeenAutoRitten",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "AutoId",
                table: "LeenAutoRitten",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeenAutoRitten",
                table: "LeenAutoRitten",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Autos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Kenteken = table.Column<string>(type: "TEXT", nullable: false),
                    Kilometerstand = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeenAutoReserveringen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EindDatum = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WerknemerId = table.Column<string>(type: "TEXT", nullable: false),
                    AutoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeenAutoReserveringen", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeenAutoReserveringen_AspNetUsers_WerknemerId",
                        column: x => x.WerknemerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeenAutoReserveringen_Autos_AutoId",
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

            migrationBuilder.CreateIndex(
                name: "IX_LeenAutoReserveringen_AutoId",
                table: "LeenAutoReserveringen",
                column: "AutoId");

            migrationBuilder.CreateIndex(
                name: "IX_LeenAutoReserveringen_WerknemerId",
                table: "LeenAutoReserveringen",
                column: "WerknemerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeenAutoRitten_AspNetUsers_WerknemerId",
                table: "LeenAutoRitten",
                column: "WerknemerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LeenAutoRitten_Autos_AutoId",
                table: "LeenAutoRitten",
                column: "AutoId",
                principalTable: "Autos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeenAutoRitten_AspNetUsers_WerknemerId",
                table: "LeenAutoRitten");

            migrationBuilder.DropForeignKey(
                name: "FK_LeenAutoRitten_Autos_AutoId",
                table: "LeenAutoRitten");

            migrationBuilder.DropTable(
                name: "LeenAutoReserveringen");

            migrationBuilder.DropTable(
                name: "Autos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeenAutoRitten",
                table: "LeenAutoRitten");

            migrationBuilder.DropIndex(
                name: "IX_LeenAutoRitten_AutoId",
                table: "LeenAutoRitten");

            migrationBuilder.DropIndex(
                name: "IX_LeenAutoRitten_WerknemerId",
                table: "LeenAutoRitten");

            migrationBuilder.DropColumn(
                name: "AankomstTijd",
                table: "LeenAutoRitten");

            migrationBuilder.DropColumn(
                name: "AutoId",
                table: "LeenAutoRitten");

            migrationBuilder.RenameTable(
                name: "LeenAutoRitten",
                newName: "LeenAutoRit");

            migrationBuilder.AlterColumn<int>(
                name: "WerknemerId",
                table: "LeenAutoRit",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeenAutoRit",
                table: "LeenAutoRit",
                column: "Id");
        }
    }
}
