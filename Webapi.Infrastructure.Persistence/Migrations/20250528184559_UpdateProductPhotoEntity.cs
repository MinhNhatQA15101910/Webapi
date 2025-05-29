using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductPhotoEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhotos_AspNetUsers_UserId",
                table: "ProductPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhotos_Products_ProductId",
                table: "ProductPhotos");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ProductPhotos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductPhotos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhotos_AspNetUsers_UserId",
                table: "ProductPhotos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhotos_Products_ProductId",
                table: "ProductPhotos",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhotos_AspNetUsers_UserId",
                table: "ProductPhotos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPhotos_Products_ProductId",
                table: "ProductPhotos");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ProductPhotos",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductId",
                table: "ProductPhotos",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhotos_AspNetUsers_UserId",
                table: "ProductPhotos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPhotos_Products_ProductId",
                table: "ProductPhotos",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
