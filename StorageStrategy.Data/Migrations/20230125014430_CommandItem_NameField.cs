using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandItemNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "CommandItems",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 24, 22, 44, 30, 173, DateTimeKind.Local).AddTicks(3358));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 24, 22, 44, 30, 173, DateTimeKind.Local).AddTicks(3383));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CommandItems",
                newName: "ProductName");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 16, 21, 38, 45, 622, DateTimeKind.Local).AddTicks(7708));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 16, 21, 38, 45, 622, DateTimeKind.Local).AddTicks(7742));
        }
    }
}
