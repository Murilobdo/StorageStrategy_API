using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandEmployeeMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Command_EmployeeId",
                table: "Command");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 15, 0, 47, 3, 11, DateTimeKind.Local).AddTicks(9026));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 15, 0, 47, 3, 11, DateTimeKind.Local).AddTicks(9052));

            migrationBuilder.CreateIndex(
                name: "IX_Command_EmployeeId",
                table: "Command",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Command_EmployeeId",
                table: "Command");

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
                name: "IX_Command_EmployeeId",
                table: "Command",
                column: "EmployeeId",
                unique: true);
        }
    }
}
