using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablePaymentMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DebitFee = table.Column<int>(type: "int", nullable: false),
                    CreditFee = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.PaymentMethodId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 8, 26, 16, 16, 8, 245, DateTimeKind.Local).AddTicks(616), new DateTime(2035, 8, 26, 16, 16, 8, 245, DateTimeKind.Local).AddTicks(638) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$QoVLa7JuX/ogAINUS3pxUA$gNDtRfA/K8BZVBspwBU2ivcn0APGVU6V1idp21dBHzY");
        }
    }
}
