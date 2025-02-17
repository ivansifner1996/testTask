using Microsoft.EntityFrameworkCore.Migrations;

namespace UpriseTask.Migrations
{
    public partial class Removingproductionandtimeseriesattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Production",
                table: "SolarPlantProduction");

            migrationBuilder.DropColumn(
                name: "TimeSeriesType",
                table: "SolarPlantProduction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Production",
                table: "SolarPlantProduction",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<short>(
                name: "TimeSeriesType",
                table: "SolarPlantProduction",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}
