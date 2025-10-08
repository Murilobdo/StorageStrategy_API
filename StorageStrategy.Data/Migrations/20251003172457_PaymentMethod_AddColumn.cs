using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentMethodAddColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "PaymentMethod",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "PaymentMethod");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 10, 3, 11, 54, 35, 427, DateTimeKind.Local).AddTicks(9574), new DateTime(2035, 10, 3, 11, 54, 35, 427, DateTimeKind.Local).AddTicks(9593) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$ewPEXsHcNx9BbS7fidGnIQ$Hf5ITLXZBy/4QYd8eYszqLFsob0ghabIDK5MIjrZ4ro");
        }
    }
}
