using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableStockHistoryMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistoryItems_StockHistory_StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems");

            migrationBuilder.DropIndex(
                name: "IX_StockHistoryItems_StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems");

            migrationBuilder.DropColumn(
                name: "StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems");

            migrationBuilder.AddColumn<int>(
                name: "StockHistoryId",
                table: "StockHistoryItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 19, 18, 54, 48, 627, DateTimeKind.Local).AddTicks(5791), new DateTime(2034, 2, 19, 18, 54, 48, 627, DateTimeKind.Local).AddTicks(5805) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$i6HmxsHEIynDrrJOIoQKZg$wmLlPAo3N9xgT25IS8WtWdz0rtzmt4ktTpO8CidYhBo");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistoryItems_StockHistoryId",
                table: "StockHistoryItems",
                column: "StockHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistoryItems_StockHistory_StockHistoryId",
                table: "StockHistoryItems",
                column: "StockHistoryId",
                principalTable: "StockHistory",
                principalColumn: "StockHistoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistoryItems_StockHistory_StockHistoryId",
                table: "StockHistoryItems");

            migrationBuilder.DropIndex(
                name: "IX_StockHistoryItems_StockHistoryId",
                table: "StockHistoryItems");

            migrationBuilder.DropColumn(
                name: "StockHistoryId",
                table: "StockHistoryItems");

            migrationBuilder.AddColumn<int>(
                name: "StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_StockHistoryItems_StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems",
                column: "StockHistoryEntityStockHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistoryItems_StockHistory_StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems",
                column: "StockHistoryEntityStockHistoryId",
                principalTable: "StockHistory",
                principalColumn: "StockHistoryId");
        }
    }
}
