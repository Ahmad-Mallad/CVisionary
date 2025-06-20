using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVisionary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoTextToPortfolio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalInfoText",
                table: "Portfolios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalInfoText",
                table: "Portfolios");
        }
    }
}
