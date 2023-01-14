using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandMappginItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandItems_Command_CommandEntityCommandId",
                table: "CommandItems");

            migrationBuilder.DropIndex(
                name: "IX_CommandItems_CommandEntityCommandId",
                table: "CommandItems");

            migrationBuilder.DropColumn(
                name: "CommandEntityCommandId",
                table: "CommandItems");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 20, 21, 4, 597, DateTimeKind.Local).AddTicks(4221));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 14, 20, 21, 4, 597, DateTimeKind.Local).AddTicks(4247));

            migrationBuilder.CreateIndex(
                name: "IX_CommandItems_CommandId",
                table: "CommandItems",
                column: "CommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandItems_Command_CommandId",
                table: "CommandItems",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "CommandId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandItems_Command_CommandId",
                table: "CommandItems");

            migrationBuilder.DropIndex(
                name: "IX_CommandItems_CommandId",
                table: "CommandItems");

            migrationBuilder.AddColumn<int>(
                name: "CommandEntityCommandId",
                table: "CommandItems",
                type: "int",
                nullable: true);

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
                name: "IX_CommandItems_CommandEntityCommandId",
                table: "CommandItems",
                column: "CommandEntityCommandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandItems_Command_CommandEntityCommandId",
                table: "CommandItems",
                column: "CommandEntityCommandId",
                principalTable: "Command",
                principalColumn: "CommandId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
