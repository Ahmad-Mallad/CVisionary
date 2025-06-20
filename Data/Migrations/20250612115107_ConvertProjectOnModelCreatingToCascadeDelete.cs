using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVisionary.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertProjectOnModelCreatingToCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Portfolios_PortfolioId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Services_ServiceId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Portfolios_PortfolioId",
                table: "Projects",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Services_ServiceId",
                table: "Projects",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Portfolios_PortfolioId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Services_ServiceId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Portfolios_PortfolioId",
                table: "Projects",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "PortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Services_ServiceId",
                table: "Projects",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId");
        }
    }
}
