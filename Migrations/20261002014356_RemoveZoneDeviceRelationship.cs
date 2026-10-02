using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveZoneDeviceRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Devices_DeviceId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_DeviceId_DisplayOrder",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_ProjectId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Zones");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_ProjectId_DisplayOrder",
                table: "Zones",
                columns: new[] { "ProjectId", "DisplayOrder" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Zones_ProjectId_DisplayOrder",
                table: "Zones");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Zones",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Zones_DeviceId_DisplayOrder",
                table: "Zones",
                columns: new[] { "DeviceId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_ProjectId",
                table: "Zones",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Devices_DeviceId",
                table: "Zones",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
