using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableStockHistoryItemAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "StockHistoryItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "StockHistoryItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 19, 18, 41, 20, 955, DateTimeKind.Local).AddTicks(2673), new DateTime(2034, 2, 19, 18, 41, 20, 955, DateTimeKind.Local).AddTicks(2687) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$OaM3bsqPql1cx31xFg11JA$koGWZai1OiaGLZNEYLfda8q8l/mcj8CAXGyom71ARIY");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "StockHistoryItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "StockHistoryItems");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 19, 14, 40, 58, 403, DateTimeKind.Local).AddTicks(4821), new DateTime(2034, 2, 19, 14, 40, 58, 403, DateTimeKind.Local).AddTicks(4836) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$L6SuoPg5MX3WjTyDG6riyQ$VjAOVTYeU9AYihYGaqKnsnQNPeM3nHPj5d0P2d0SQS0");
        }
    }
}
