using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableStockHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockHistory",
                columns: table => new
                {
                    StockHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistory", x => x.StockHistoryId);
                    table.ForeignKey(
                        name: "FK_StockHistory_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockHistoryItems",
                columns: table => new
                {
                    StockHistoryItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Taxing = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockHistoryEntityStockHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistoryItems", x => x.StockHistoryItemId);
                    table.ForeignKey(
                        name: "FK_StockHistoryItems_StockHistory_StockHistoryEntityStockHistoryId",
                        column: x => x.StockHistoryEntityStockHistoryId,
                        principalTable: "StockHistory",
                        principalColumn: "StockHistoryId");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_StockHistory_CompanyId",
                table: "StockHistory",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistoryItems_StockHistoryEntityStockHistoryId",
                table: "StockHistoryItems",
                column: "StockHistoryEntityStockHistoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockHistoryItems");

            migrationBuilder.DropTable(
                name: "StockHistory");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 9, 15, 4, 56, 381, DateTimeKind.Local).AddTicks(7968), new DateTime(2034, 2, 9, 15, 4, 56, 381, DateTimeKind.Local).AddTicks(7983) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$Iz7qfapW3a6ZMft6+5kyMg$AEmUtvp1guS6cLzDGqCXy5u8VyisFaEVDP4Z/R/30tQ");
        }
    }
}
