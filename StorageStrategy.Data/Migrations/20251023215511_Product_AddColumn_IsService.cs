using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductAddColumnIsService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsService",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 10, 23, 18, 55, 10, 738, DateTimeKind.Local).AddTicks(8175), new DateTime(2035, 10, 23, 18, 55, 10, 738, DateTimeKind.Local).AddTicks(8193) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$e5HKd4T+V4TQYHSr70Yt1g$e/8By8dyu1OG6tFzBE6nO15VfjgHscs0xjF8xcDxKvM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsService",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 10, 3, 15, 13, 59, 906, DateTimeKind.Local).AddTicks(8668), new DateTime(2035, 10, 3, 15, 13, 59, 906, DateTimeKind.Local).AddTicks(8685) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$WLPidp1JKT/IGNF4VafScw$Uj342cBGCpCsnCIrcXnxLnSImvXqlKg4WFMCb+xeZb0");
        }
    }
}
