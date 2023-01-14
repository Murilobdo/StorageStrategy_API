using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommandId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Command",
                columns: table => new
                {
                    CommandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payment = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.CommandId);
                    table.ForeignKey(
                        name: "FK_Command_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 1, 26, 15, 898, DateTimeKind.Local).AddTicks(7876));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 1, 26, 15, 898, DateTimeKind.Local).AddTicks(7897));

            migrationBuilder.CreateIndex(
                name: "IX_Product_CommandId",
                table: "Product",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_Command_EmployeeId",
                table: "Command",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Command_CommandId",
                table: "Product",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "CommandId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Command_CommandId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.DropIndex(
                name: "IX_Product_CommandId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CommandId",
                table: "Product");

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
    }
}
