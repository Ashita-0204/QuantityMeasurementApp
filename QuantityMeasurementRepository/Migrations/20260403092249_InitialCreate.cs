using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace QuantityMeasurementRepository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "quantity_measurements_ef",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Operation = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Operand1Value = table.Column<double>(type: "double precision", nullable: true),
                    Operand1Unit = table.Column<string>(type: "text", nullable: true),
                    Operand1Category = table.Column<string>(type: "text", nullable: true),
                    Operand2Value = table.Column<double>(type: "double precision", nullable: true),
                    Operand2Unit = table.Column<string>(type: "text", nullable: true),
                    Operand2Category = table.Column<string>(type: "text", nullable: true),
                    ResultValue = table.Column<double>(type: "double precision", nullable: true),
                    ResultUnit = table.Column<string>(type: "text", nullable: true),
                    ResultCategory = table.Column<string>(type: "text", nullable: true),
                    BoolResult = table.Column<bool>(type: "boolean", nullable: true),
                    ScalarResult = table.Column<double>(type: "double precision", nullable: true),
                    HasError = table.Column<bool>(type: "boolean", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quantity_measurements_ef", x => x.Id);
                    table.ForeignKey(
                        name: "FK_quantity_measurements_ef_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_quantity_measurements_ef_UserId",
                table: "quantity_measurements_ef",
                column: "UserId");

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
