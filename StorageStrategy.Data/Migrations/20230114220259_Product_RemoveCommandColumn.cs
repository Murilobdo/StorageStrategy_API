using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductRemoveCommandColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Command_CommandId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CommandId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CommandId",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "CommandEntityCommandId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 19, 2, 59, 633, DateTimeKind.Local).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 19, 2, 59, 633, DateTimeKind.Local).AddTicks(9785));

            migrationBuilder.CreateIndex(
                name: "IX_Product_CommandEntityCommandId",
                table: "Product",
                column: "CommandEntityCommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Command_CommandEntityCommandId",
                table: "Product",
                column: "CommandEntityCommandId",
                principalTable: "Command",
                principalColumn: "CommandId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Command_CommandEntityCommandId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CommandEntityCommandId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CommandEntityCommandId",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "CommandId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 18, 46, 49, 872, DateTimeKind.Local).AddTicks(7654));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 18, 46, 49, 872, DateTimeKind.Local).AddTicks(7683));

            migrationBuilder.CreateIndex(
                name: "IX_Product_CommandId",
                table: "Product",
                column: "CommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Command_CommandId",
                table: "Product",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "CommandId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
