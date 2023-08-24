using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosTblVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la Villa...", new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2718), new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2708), "", 200, "Premium vista a la Piscina", 10, 400.0 },
                    { 2, "", "Detalle de la Villa...", new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2721), new DateTime(2023, 8, 24, 12, 32, 35, 927, DateTimeKind.Local).AddTicks(2720), "", 100, "Villa Real", 5, 200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
