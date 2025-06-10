using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSizeIdForCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductSizeId",
                table: "CartItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductSizeId",
                table: "CartItems",
                column: "ProductSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                table: "CartItems",
                column: "ProductSizeId",
                principalTable: "ProductSizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductSizes_ProductSizeId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductSizeId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ProductSizeId",
                table: "CartItems");
        }
    }
}
