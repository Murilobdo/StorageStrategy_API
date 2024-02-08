using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemCommandAddTaxing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Taxing",
                table: "CommandItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 12, 19, 20, 46, 54, 985, DateTimeKind.Local).AddTicks(5328), new DateTime(2033, 12, 19, 20, 46, 54, 985, DateTimeKind.Local).AddTicks(5342) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$oTVuIZC1J3lDIQfdYm3Kiw$nUoWazZm9rI7VqlfBZCm7vBQuND/c4t1Up7rtqQscpk");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Taxing",
                table: "CommandItems");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 11, 22, 20, 19, 14, 824, DateTimeKind.Local).AddTicks(2660), new DateTime(2033, 11, 22, 20, 19, 14, 824, DateTimeKind.Local).AddTicks(2673) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$QM2qarRh03znC3TUt70yuA$SpQhQ0ewony1nRD21lUF2DF+UTK2sNxjIPOn/d9TX6I");
        }
    }
}
