using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddVoucherItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherItem_Vouchers_VoucherId1",
                table: "VoucherItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoucherItem",
                table: "VoucherItem");

            migrationBuilder.DropIndex(
                name: "IX_VoucherItem_VoucherId1",
                table: "VoucherItem");

            migrationBuilder.DropColumn(
                name: "VoucherId1",
                table: "VoucherItem");

            migrationBuilder.RenameTable(
                name: "VoucherItem",
                newName: "VoucherItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoucherItems",
                table: "VoucherItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherItems_VoucherId",
                table: "VoucherItems",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherItems_Vouchers_VoucherId",
                table: "VoucherItems",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VoucherItems_Vouchers_VoucherId",
                table: "VoucherItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoucherItems",
                table: "VoucherItems");

            migrationBuilder.DropIndex(
                name: "IX_VoucherItems_VoucherId",
                table: "VoucherItems");

            migrationBuilder.RenameTable(
                name: "VoucherItems",
                newName: "VoucherItem");

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherId1",
                table: "VoucherItem",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoucherItem",
                table: "VoucherItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherItem_VoucherId1",
                table: "VoucherItem",
                column: "VoucherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherItem_Vouchers_VoucherId1",
                table: "VoucherItem",
                column: "VoucherId1",
                principalTable: "Vouchers",
                principalColumn: "Id");
        }
    }
}
