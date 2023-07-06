using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class MudandoSenha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$cPKqotj4IgYM6lk7GyylRw$iI5cTDirQMO18egBisojUQbhPf5OX+zS+C+STNMxg3k");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 6, 13, 22, 50, 27, 36, DateTimeKind.Local).AddTicks(1121), new DateTime(2033, 6, 13, 22, 50, 27, 36, DateTimeKind.Local).AddTicks(1138) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$tz1kynOTjrTCNkPi8qtvew$4x5/gOjHs7F+pkdn+ALpHIIol9lJchf3I56n53ykuVY");
        }
    }
}
