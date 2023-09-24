using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseTableArrumandoPrecisaoValorDespesa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                values: new object[] { new DateTime(2023, 9, 24, 16, 31, 0, 97, DateTimeKind.Local).AddTicks(9692), new DateTime(2033, 9, 24, 16, 31, 0, 97, DateTimeKind.Local).AddTicks(9707) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$Sn2CZpzPCkgOPPWw28JMQA$XkzaO5d6BJ11FFeEzBi0qZLDIJ5AH2EgQpe6ZO4fUwA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
