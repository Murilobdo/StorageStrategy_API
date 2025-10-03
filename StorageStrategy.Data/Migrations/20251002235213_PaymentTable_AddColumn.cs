using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTableAddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CreditFee",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DebitFee",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 10, 2, 20, 52, 12, 293, DateTimeKind.Local).AddTicks(8984), new DateTime(2035, 10, 2, 20, 52, 12, 293, DateTimeKind.Local).AddTicks(9004) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$0hFd1yR3B43KRgPfVpUzjw$XscGhUHcJ37e4TfTjk8AvxbQSwDUNvySaFBfPJllqHk");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_PaymentMethod_PaymentMethodId",
                table: "Payment",
                column: "PaymentMethodId",
                principalTable: "PaymentMethod",
                principalColumn: "PaymentMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_PaymentMethod_PaymentMethodId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PaymentMethodId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "CreditFee",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "DebitFee",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Payment");

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
        }
    }
}
