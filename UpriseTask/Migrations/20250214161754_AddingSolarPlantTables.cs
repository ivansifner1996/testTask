using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UpriseTask.Migrations
{
    public partial class AddingSolarPlantTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolarPlant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstalledPower = table.Column<double>(type: "float", nullable: false),
                    DateOfInstallation = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPlant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolarPlantProduction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SolarPlantId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Production = table.Column<double>(type: "float", nullable: false),
                    TimeSeriesType = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolarPlantProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolarPlantProduction_SolarPlant_SolarPlantId",
                        column: x => x.SolarPlantId,
                        principalTable: "SolarPlant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolarPlantProduction_SolarPlantId",
                table: "SolarPlantProduction",
                column: "SolarPlantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolarPlantProduction");

            migrationBuilder.DropTable(
                name: "SolarPlant");
        }
    }
}
