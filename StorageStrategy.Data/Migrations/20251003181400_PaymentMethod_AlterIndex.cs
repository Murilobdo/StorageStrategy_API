using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentMethodAlterIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_CompanyId_Company",
                table: "PaymentMethod");

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

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_Company_Method",
                table: "PaymentMethod",
                columns: new[] { "Company", "Method" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_Company_Method",
                table: "PaymentMethod");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 10, 3, 14, 24, 55, 997, DateTimeKind.Local).AddTicks(245), new DateTime(2035, 10, 3, 14, 24, 55, 997, DateTimeKind.Local).AddTicks(264) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$mfdPFQ7fBG7FsAyFmZw8Cw$JMaXXK3gcA+QJ26jLum3t4tZYuGW1pal912NPxc/JG4");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_CompanyId_Company",
                table: "PaymentMethod",
                columns: new[] { "CompanyId", "Company" },
                unique: true);
        }
    }
}
