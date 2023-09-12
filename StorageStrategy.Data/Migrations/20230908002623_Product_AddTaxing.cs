using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ProductAddTaxing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Taxing",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpenseValue",
                table: "Expenses",
                type: "decimal(4,2)",
                precision: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 9, 7, 21, 26, 22, 223, DateTimeKind.Local).AddTicks(2753), new DateTime(2033, 9, 7, 21, 26, 22, 223, DateTimeKind.Local).AddTicks(2768) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$Z6q5fq0SDQUUVTsKBvOcvw$GY3faDO/fOUcaKCv9SD/5L2YLzqP1qQu9eC6FcS+ZxQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Taxing",
                table: "Product");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExpenseValue",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(4,2)",
                oldPrecision: 4);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 8, 13, 12, 36, 24, 423, DateTimeKind.Local).AddTicks(3412), new DateTime(2033, 8, 13, 12, 36, 24, 423, DateTimeKind.Local).AddTicks(3426) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$g0A9Ac1SgLWMQSYC+XHnrQ$q7FaM8QP3BiAm2hHzDwHoYumfnWe3HTXf4Ja+5Yt3Kc");
        }
    }
}
