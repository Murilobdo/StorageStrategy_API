using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandFinalDateNulale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FinalDate",
                table: "Command",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "FinalDate",
                table: "Command",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

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
        }
    }
}
