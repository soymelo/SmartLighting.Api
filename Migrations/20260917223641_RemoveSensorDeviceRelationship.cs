using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSensorDeviceRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM Sensors AS s
                    INNER JOIN Zones AS z ON z.Id = s.ZoneId
                    WHERE s.DeviceId <> z.DeviceId
                )
                BEGIN
                    THROW 50000, 'A relação entre Sensor e Zone está inconsistente. Corrija os dados antes de remover Sensor.DeviceId.', 1;
                END;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Devices_DeviceId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DeviceId_SensorIdentifier",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_ZoneId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Sensors");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ZoneId_SensorIdentifier",
                table: "Sensors",
                columns: new[] { "ZoneId", "SensorIdentifier" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sensors_ZoneId_SensorIdentifier",
                table: "Sensors");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Sensors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceId_SensorIdentifier",
                table: "Sensors",
                columns: new[] { "DeviceId", "SensorIdentifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ZoneId",
                table: "Sensors",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Devices_DeviceId",
                table: "Sensors",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
