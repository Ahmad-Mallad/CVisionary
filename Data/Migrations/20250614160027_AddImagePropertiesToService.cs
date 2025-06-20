using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVisionary.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePropertiesToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ServiceImage",
                table: "Services",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceImageName",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceImageType",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceImage",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceImageName",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceImageType",
                table: "Services");
        }
    }
}
