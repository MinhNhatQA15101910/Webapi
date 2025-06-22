using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webapi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VoucherEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Vouchers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vouchers",
                newName: "TypeId");

            migrationBuilder.CreateTable(
                name: "VoucherItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    VoucherId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<bool>(type: "INTEGER", nullable: false),
                    VoucherId1 = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherItem_Vouchers_VoucherId1",
                        column: x => x.VoucherId1,
                        principalTable: "Vouchers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoucherType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_TypeId",
                table: "Vouchers",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherItem_VoucherId1",
                table: "VoucherItem",
                column: "VoucherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_VoucherType_TypeId",
                table: "Vouchers",
                column: "TypeId",
                principalTable: "VoucherType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_VoucherType_TypeId",
                table: "Vouchers");

            migrationBuilder.DropTable(
                name: "VoucherItem");

            migrationBuilder.DropTable(
                name: "VoucherType");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_TypeId",
                table: "Vouchers");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Vouchers",
                newName: "Name");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Vouchers",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
