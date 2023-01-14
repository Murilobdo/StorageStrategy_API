using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Command",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Command",
                newName: "InitialDate");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Command",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalDate",
                table: "Command",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Command",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Command",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                name: "IX_Command_CompanyId",
                table: "Command",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Command_Company_CompanyId",
                table: "Command",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Command_Company_CompanyId",
                table: "Command");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Command_CompanyId",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "FinalDate",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Command");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Command",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "InitialDate",
                table: "Command",
                newName: "Date");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Company_CompanyId",
                table: "Employee",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
