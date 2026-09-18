using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class RenameTopologyIdentifiersToNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Zones_DeviceId_ZoneIdentifier",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_ZoneId_SensorIdentifier",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Luminaires_ZoneId_LuminaireIdentifier",
                table: "Luminaires");

            migrationBuilder.DropIndex(
                name: "IX_Devices_DeviceIdentifier",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "ZoneIdentifier",
                table: "Zones",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SensorIdentifier",
                table: "Sensors",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LuminaireIdentifier",
                table: "Luminaires",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DeviceIdentifier",
                table: "Devices",
                newName: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ZoneId",
                table: "Sensors",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Luminaires_ZoneId",
                table: "Luminaires",
                column: "ZoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensors_ZoneId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Luminaires_ZoneId",
                table: "Luminaires");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Zones",
                newName: "ZoneIdentifier");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sensors",
                newName: "SensorIdentifier");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Luminaires",
                newName: "LuminaireIdentifier");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Devices",
                newName: "DeviceIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_DeviceId_ZoneIdentifier",
                table: "Zones",
                columns: new[] { "DeviceId", "ZoneIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ZoneId_SensorIdentifier",
                table: "Sensors",
                columns: new[] { "ZoneId", "SensorIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Luminaires_ZoneId_LuminaireIdentifier",
                table: "Luminaires",
                columns: new[] { "ZoneId", "LuminaireIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_DeviceIdentifier",
                table: "Devices",
                column: "DeviceIdentifier",
                unique: true);
        }
    }
}
