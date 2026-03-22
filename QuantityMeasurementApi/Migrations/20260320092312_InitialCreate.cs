using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quantity_measurements_ef",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Operand1Value = table.Column<double>(type: "float", nullable: true),
                    Operand1Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operand1Category = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Operand2Value = table.Column<double>(type: "float", nullable: true),
                    Operand2Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Operand2Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultValue = table.Column<double>(type: "float", nullable: true),
                    ResultUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoolResult = table.Column<bool>(type: "bit", nullable: true),
                    ScalarResult = table.Column<double>(type: "float", nullable: true),
                    HasError = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quantity_measurements_ef", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_ef_Operand1Category",
                table: "quantity_measurements_ef",
                column: "Operand1Category");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_ef_Operation",
                table: "quantity_measurements_ef",
                column: "Operation");

            migrationBuilder.CreateIndex(
                name: "IX_quantity_measurements_ef_Timestamp",
                table: "quantity_measurements_ef",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Username",
                table: "users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quantity_measurements_ef");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
