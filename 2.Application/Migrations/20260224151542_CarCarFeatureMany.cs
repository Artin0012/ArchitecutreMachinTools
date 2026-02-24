using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.Application.Migrations
{
    /// <inheritdoc />
    public partial class CarCarFeatureMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Cars_CarId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_CarId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Features");

            migrationBuilder.CreateTable(
                name: "CarFeatureMappings",
                columns: table => new
                {
                    CarsId = table.Column<int>(type: "int", nullable: false),
                    FeaturesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFeatureMappings", x => new { x.CarsId, x.FeaturesId });
                    table.ForeignKey(
                        name: "FK_CarFeatureMappings_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarFeatureMappings_Features_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarFeatureMappings_FeaturesId",
                table: "CarFeatureMappings",
                column: "FeaturesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarFeatureMappings");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Features",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Features_CarId",
                table: "Features",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Cars_CarId",
                table: "Features",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
