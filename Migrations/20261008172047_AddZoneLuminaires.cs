using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartLighting.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddZoneLuminaires : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Luminaires_Zones_ZoneId",
                table: "Luminaires");

            migrationBuilder.CreateTable(
                name: "ZoneLuminaires",
                columns: table => new
                {
                    ZoneId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LuminaireId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneLuminaires", x => new { x.ZoneId, x.LuminaireId });
                    table.ForeignKey(
                        name: "FK_ZoneLuminaires_Luminaires_LuminaireId",
                        column: x => x.LuminaireId,
                        principalTable: "Luminaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZoneLuminaires_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZoneLuminaires_LuminaireId",
                table: "ZoneLuminaires",
                column: "LuminaireId");

            migrationBuilder.Sql("""
                INSERT INTO [ZoneLuminaires] ([ZoneId], [LuminaireId])
                SELECT [ZoneId], [Id]
                FROM [Luminaires];
                """);

            migrationBuilder.DropIndex(
                name: "IX_Luminaires_ZoneId",
                table: "Luminaires");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Luminaires");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (
                    SELECT 1
                    FROM [Luminaires] AS l
                    LEFT JOIN [ZoneLuminaires] AS zl ON zl.[LuminaireId] = l.[Id]
                    GROUP BY l.[Id]
                    HAVING COUNT(zl.[ZoneId]) <> 1
                )
                    THROW 50001, 'A reversao exige exatamente uma zona por luminaria.', 1;
                """);

            migrationBuilder.AddColumn<Guid>(
                name: "ZoneId",
                table: "Luminaires",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE l
                SET l.[ZoneId] = zl.[ZoneId]
                FROM [Luminaires] AS l
                INNER JOIN [ZoneLuminaires] AS zl ON zl.[LuminaireId] = l.[Id];
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "ZoneId",
                table: "Luminaires",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.DropTable(
                name: "ZoneLuminaires");

            migrationBuilder.CreateIndex(
                name: "IX_Luminaires_ZoneId",
                table: "Luminaires",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Luminaires_Zones_ZoneId",
                table: "Luminaires",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
