using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CompanyValidateDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Validate",
                table: "Company",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9811), new DateTime(2033, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9828) });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9849), new DateTime(2026, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9849) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validate",
                table: "Company");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 2, 27, 20, 24, 53, 869, DateTimeKind.Local).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 2, 27, 20, 24, 53, 869, DateTimeKind.Local).AddTicks(3745));
        }
    }
}
