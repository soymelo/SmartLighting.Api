using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTelemetryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastTelemetryAtUtc",
                table: "Devices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LuminaireStateChanges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LuminaireId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrightnessPercentage = table.Column<int>(type: "int", nullable: false),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuminaireStateChanges", x => x.Id);
                    table.CheckConstraint("CK_LuminaireStateChanges_BrightnessPercentage", "[BrightnessPercentage] BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_LuminaireStateChanges_Luminaires_LuminaireId",
                        column: x => x.LuminaireId,
                        principalTable: "Luminaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LuminaireStates",
                columns: table => new
                {
                    LuminaireId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BrightnessPercentage = table.Column<int>(type: "int", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LuminaireStates", x => x.LuminaireId);
                    table.CheckConstraint("CK_LuminaireStates_BrightnessPercentage", "[BrightnessPercentage] BETWEEN 0 AND 100");
                    table.ForeignKey(
                        name: "FK_LuminaireStates_Luminaires_LuminaireId",
                        column: x => x.LuminaireId,
                        principalTable: "Luminaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorEvents_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LuminaireStateChanges_EventIdentifier",
                table: "LuminaireStateChanges",
                column: "EventIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LuminaireStateChanges_LuminaireId_ChangedAtUtc",
                table: "LuminaireStateChanges",
                columns: new[] { "LuminaireId", "ChangedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_SensorEvents_EventIdentifier",
                table: "SensorEvents",
                column: "EventIdentifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SensorEvents_SensorId_OccurredAtUtc",
                table: "SensorEvents",
                columns: new[] { "SensorId", "OccurredAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LuminaireStateChanges");

            migrationBuilder.DropTable(
                name: "LuminaireStates");

            migrationBuilder.DropTable(
                name: "SensorEvents");

            migrationBuilder.DropColumn(
                name: "LastTelemetryAtUtc",
                table: "Devices");
        }
    }
}
