using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab6_6minimalapi_antonov.Migrations
{
    /// <inheritdoc />
    public partial class SeedCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "CreatedAt", "IsAvailable", "Model", "PricePerDay", "Year" },
                values: new object[,]
                {
                    { 1, "Toyota", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Camry", 4500m, 2021 },
                    { 2, "BMW", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), true, "X5", 8000m, 2020 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cars",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
