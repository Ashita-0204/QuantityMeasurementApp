using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementApi.Migrations
{
    /// <summary>
    /// Fixes the UserId column:
    ///  1. Alter UserId from INT NOT NULL (default 0) → INT NULL
    ///     — this sets all existing rows that had UserId=0 to NULL, removing
    ///       the orphan-row violation that caused every SaveChangesAsync to fail.
    ///  2. Adds the real FK constraint UserId → users.Id
    ///
    /// Run: dotnet ef database update --project QuantityMeasurementApi
    /// </summary>
    public partial class FixUserIdNullableAndAddFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Null out any rows where UserId=0 (no real user owns them)
            migrationBuilder.Sql(
                "UPDATE [quantity_measurements_ef] SET [UserId] = NULL WHERE [UserId] = 0 OR [UserId] NOT IN (SELECT [Id] FROM [users])");

            // Step 2: Alter column to nullable INT
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "quantity_measurements_ef",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: false);

            // Step 3: Add FK constraint (now safe — no orphan rows remain)
            migrationBuilder.AddForeignKey(
                name: "FK_quantity_measurements_ef_users_UserId",
                table: "quantity_measurements_ef",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quantity_measurements_ef_users_UserId",
                table: "quantity_measurements_ef");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "quantity_measurements_ef",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}