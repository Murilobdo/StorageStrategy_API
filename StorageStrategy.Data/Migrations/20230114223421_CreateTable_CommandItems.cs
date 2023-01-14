using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableCommandItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CommandItems",
                columns: table => new
                {
                    CommandItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Qtd = table.Column<int>(type: "int", nullable: false),
                    CommandEntityCommandId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandItems", x => x.CommandItemId);
                    table.ForeignKey(
                        name: "FK_CommandItems_Command_CommandEntityCommandId",
                        column: x => x.CommandEntityCommandId,
                        principalTable: "Command",
                        principalColumn: "CommandId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CommandItems_CommandEntityCommandId",
                table: "CommandItems",
                column: "CommandEntityCommandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandItems");

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
    }
}
