using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "quantity_measurements_ef",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_ef_UserId",
                table: "quantity_measurements_ef",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_quantity_measurements_ef_UserId",
                table: "quantity_measurements_ef");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "quantity_measurements_ef");
        }
    }
}