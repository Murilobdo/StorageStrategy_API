using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductStockAlert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Qtd",
                table: "Product",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "StockAlert",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 6, 17, 56, 47, 294, DateTimeKind.Local).AddTicks(3484));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 6, 17, 56, 47, 294, DateTimeKind.Local).AddTicks(3503));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockAlert",
                table: "Product");

            migrationBuilder.AlterColumn<decimal>(
                name: "Qtd",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2022, 12, 26, 16, 45, 12, 831, DateTimeKind.Local).AddTicks(7673));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2022, 12, 26, 16, 45, 12, 831, DateTimeKind.Local).AddTicks(7694));
        }
    }
}
