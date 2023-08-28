using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTablaNumeroVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "numeroVillas",
                columns: table => new
                {
                    NumVilla = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCrecion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeroVillas", x => x.NumVilla);
                    table.ForeignKey(
                        name: "FK_numeroVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 8, 28, 9, 26, 13, 613, DateTimeKind.Local).AddTicks(7201), new DateTime(2023, 8, 28, 9, 26, 13, 613, DateTimeKind.Local).AddTicks(7200) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 8, 28, 9, 26, 13, 613, DateTimeKind.Local).AddTicks(7204), new DateTime(2023, 8, 28, 9, 26, 13, 613, DateTimeKind.Local).AddTicks(7204) });

            migrationBuilder.CreateIndex(
                name: "IX_numeroVillas_VillaId",
                table: "numeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "numeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2718), new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2708) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FechaActualizacion", "FechaCreacion" },
                values: new object[] { new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2721), new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2720) });
        }
    }
}
