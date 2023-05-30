using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJobRoleToSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 5, 30, 13, 19, 14, 394, DateTimeKind.Local).AddTicks(8606), new DateTime(2033, 5, 30, 13, 19, 14, 394, DateTimeKind.Local).AddTicks(8622) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "JobRole", "PasswordHash" },
                values: new object[] { "Developer", "$argon2id$v=19$m=65536,t=3,p=1$XKxsutIKm2KWJm2k+4Wkbw$v/5iZExYOFfp+n4tzcr6IeNXeBwYVHKN0TqC0jPXOGI" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 5, 30, 13, 17, 21, 410, DateTimeKind.Local).AddTicks(539), new DateTime(2033, 5, 30, 13, 17, 21, 410, DateTimeKind.Local).AddTicks(556) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "JobRole", "PasswordHash" },
                values: new object[] { "", "$argon2id$v=19$m=65536,t=3,p=1$2GWZBcDZrThSnj/D+oNKpw$RKhP3QU8S4Tuur6pY+e1aIqGAqIy2Mt2cdXTDsGiM7Q" });
        }
    }
}
