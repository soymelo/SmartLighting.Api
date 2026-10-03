using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDeviceComponentSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensors_DeviceId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Luminaires_DeviceId",
                table: "Luminaires");

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Slot",
                table: "Luminaires",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceId_Slot",
                table: "Sensors",
                columns: new[] { "DeviceId", "Slot" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Sensors_Slot",
                table: "Sensors",
                sql: "[Slot] IN (1, 2, 3)");

            migrationBuilder.CreateIndex(
                name: "IX_Luminaires_DeviceId_Slot",
                table: "Luminaires",
                columns: new[] { "DeviceId", "Slot" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Luminaires_Slot",
                table: "Luminaires",
                sql: "[Slot] IN (1, 2, 3, 4, 5, 6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensors_DeviceId_Slot",
                table: "Sensors");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Sensors_Slot",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Luminaires_DeviceId_Slot",
                table: "Luminaires");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Luminaires_Slot",
                table: "Luminaires");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "Slot",
                table: "Luminaires");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceId",
                table: "Sensors",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Luminaires_DeviceId",
                table: "Luminaires",
                column: "DeviceId");
        }
    }
}
