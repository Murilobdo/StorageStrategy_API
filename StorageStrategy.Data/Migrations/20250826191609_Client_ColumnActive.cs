using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientColumnActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Client",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Client");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 6, 22, 14, 42, 26, 240, DateTimeKind.Local).AddTicks(9827), new DateTime(2035, 6, 22, 14, 42, 26, 240, DateTimeKind.Local).AddTicks(9845) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$R7+Jz6t7OtWIvYCOaGylRw$Xev79b3VgRA6fLnE51JOsm3dvugWq+kpG7qITOA1k9E");
        }
    }
}
