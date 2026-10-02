using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectsAndDeviceOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Zones",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Sensors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "SensorEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "LuminaireStateChanges",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Luminaires",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Devices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_ProjectId",
                table: "Zones",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceId",
                table: "Sensors",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorEvents_DeviceId",
                table: "SensorEvents",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_LuminaireStateChanges_DeviceId",
                table: "LuminaireStateChanges",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Luminaires_DeviceId",
                table: "Luminaires",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ProjectId",
                table: "Devices",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Projects_ProjectId",
                table: "Devices",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Luminaires_Devices_DeviceId",
                table: "Luminaires",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LuminaireStateChanges_Devices_DeviceId",
                table: "LuminaireStateChanges",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SensorEvents_Devices_DeviceId",
                table: "SensorEvents",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Devices_DeviceId",
                table: "Sensors",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Projects_ProjectId",
                table: "Zones",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Projects_ProjectId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Luminaires_Devices_DeviceId",
                table: "Luminaires");

            migrationBuilder.DropForeignKey(
                name: "FK_LuminaireStateChanges_Devices_DeviceId",
                table: "LuminaireStateChanges");

            migrationBuilder.DropForeignKey(
                name: "FK_SensorEvents_Devices_DeviceId",
                table: "SensorEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Devices_DeviceId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Projects_ProjectId",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Zones_ProjectId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DeviceId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_SensorEvents_DeviceId",
                table: "SensorEvents");

            migrationBuilder.DropIndex(
                name: "IX_LuminaireStateChanges_DeviceId",
                table: "LuminaireStateChanges");

            migrationBuilder.DropIndex(
                name: "IX_Luminaires_DeviceId",
                table: "Luminaires");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ProjectId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "SensorEvents");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "LuminaireStateChanges");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Luminaires");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Devices");
        }
    }
}
