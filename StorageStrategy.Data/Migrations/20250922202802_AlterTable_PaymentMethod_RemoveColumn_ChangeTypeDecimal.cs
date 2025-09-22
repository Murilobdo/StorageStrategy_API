using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTablePaymentMethodRemoveColumnChangeTypeDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_CompanyId_Name",
                table: "PaymentMethod");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "PaymentMethod");

            migrationBuilder.AlterColumn<decimal>(
                name: "DebitFee",
                table: "PaymentMethod",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "CreditFee",
                table: "PaymentMethod",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "PaymentMethod",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 9, 22, 17, 28, 1, 82, DateTimeKind.Local).AddTicks(3676), new DateTime(2035, 9, 22, 17, 28, 1, 82, DateTimeKind.Local).AddTicks(3693) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$syEbJQgXdR054AvgiE4mZw$ffFJry3a/JYwY1lXoGOH0CqXwq9xac4jn2YhMfWJJMk");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_CompanyId_Company",
                table: "PaymentMethod",
                columns: new[] { "CompanyId", "Company" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_CompanyId_Company",
                table: "PaymentMethod");

            migrationBuilder.AlterColumn<int>(
                name: "DebitFee",
                table: "PaymentMethod",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "CreditFee",
                table: "PaymentMethod",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Company",
                table: "PaymentMethod",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "PaymentMethod",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 9, 22, 17, 10, 14, 605, DateTimeKind.Local).AddTicks(608), new DateTime(2035, 9, 22, 17, 10, 14, 605, DateTimeKind.Local).AddTicks(625) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$7f5cfITYPw2Kf5wIcVa04A$18C9McIFE+hjfE8ber0LMPH2YoMyKo34SMm7v+Bf3Lo");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_CompanyId_Name",
                table: "PaymentMethod",
                columns: new[] { "CompanyId", "Name" },
                unique: true);
        }
    }
}
