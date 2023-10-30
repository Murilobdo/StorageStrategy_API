using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseAddValueExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExpenseValue",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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
                columns: new[] { "Email", "PasswordHash" },
                values: new object[] { "murilobdo@admin.com", "$argon2id$v=19$m=65536,t=3,p=1$g0A9Ac1SgLWMQSYC+XHnrQ$q7FaM8QP3BiAm2hHzDwHoYumfnWe3HTXf4Ja+5Yt3Kc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseValue",
                table: "Expenses");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 7, 5, 19, 47, 15, 575, DateTimeKind.Local).AddTicks(7964), new DateTime(2033, 7, 5, 19, 47, 15, 575, DateTimeKind.Local).AddTicks(7982) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "Email", "PasswordHash" },
                values: new object[] { "murilobdo@admin.com.br", "$argon2id$v=19$m=65536,t=3,p=1$cPKqotj4IgYM6lk7GyylRw$iI5cTDirQMO18egBisojUQbhPf5OX+zS+C+STNMxg3k" });
        }
    }
}
