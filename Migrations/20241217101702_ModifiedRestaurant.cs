using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodie.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_bio_users_user_id",
                table: "users_bio");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "users_bio",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "users_bio",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AddColumn<decimal>(
                name: "latitude",
                table: "restaurants",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "longitude",
                table: "restaurants",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "restaurant_address",
                table: "restaurants",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "restaurant_contact",
                table: "restaurants",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "restaurant_website",
                table: "restaurants",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_users_bio_users_user_id",
                table: "users_bio",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_bio_users_user_id",
                table: "users_bio");

            migrationBuilder.DropColumn(
                name: "latitude",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "restaurant_address",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "restaurant_contact",
                table: "restaurants");

            migrationBuilder.DropColumn(
                name: "restaurant_website",
                table: "restaurants");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "users_bio",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bio",
                table: "users_bio",
                type: "character varying(1500)",
                maxLength: 1500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1500)",
                oldMaxLength: 1500,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_bio_users_user_id",
                table: "users_bio",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
