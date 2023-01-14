using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandItemAddProductId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CommandItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 19, 39, 46, 672, DateTimeKind.Local).AddTicks(3261));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 19, 39, 46, 672, DateTimeKind.Local).AddTicks(3285));

            migrationBuilder.CreateIndex(
                name: "IX_CommandItems_ProductId",
                table: "CommandItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandItems_Product_ProductId",
                table: "CommandItems",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandItems_Product_ProductId",
                table: "CommandItems");

            migrationBuilder.DropIndex(
                name: "IX_CommandItems_ProductId",
                table: "CommandItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CommandItems");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 19, 34, 21, 527, DateTimeKind.Local).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 19, 34, 21, 527, DateTimeKind.Local).AddTicks(4434));
        }
    }
}
