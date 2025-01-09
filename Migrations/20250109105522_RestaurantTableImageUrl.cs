using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodie.Migrations
{
    /// <inheritdoc />
    public partial class RestaurantTableImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "restaurants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "restaurant_description",
                table: "restaurants",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "restaurant_id",
                table: "restaurants",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "restaurant_map_link",
                table: "restaurants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "restaurant_description",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "restaurant_id",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "restaurant_map_link",
                table: "restaurants");
        }
    }
}
