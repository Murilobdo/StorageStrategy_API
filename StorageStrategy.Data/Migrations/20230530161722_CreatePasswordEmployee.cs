using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreatePasswordEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Description", "Name", "Validate" },
                values: new object[] { new DateTime(2023, 5, 30, 13, 17, 21, 410, DateTimeKind.Local).AddTicks(539), "Admin", "Admin Company", new DateTime(2033, 5, 30, 13, 17, 21, 410, DateTimeKind.Local).AddTicks(556) });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "Comission", "CompanyId", "Email", "IsActive", "JobRole", "Name", "PasswordHash" },
                values: new object[] { 1, 0, 1, "murilobdo@admin.com.br", true, "", "Murilo Bernardes (Admin)", "$argon2id$v=19$m=65536,t=3,p=1$2GWZBcDZrThSnj/D+oNKpw$RKhP3QU8S4Tuur6pY+e1aIqGAqIy2Mt2cdXTDsGiM7Q" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Employee");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Description", "Name", "Validate" },
                values: new object[] { new DateTime(2023, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9811), "Bar", "Bar do Murps", new DateTime(2033, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9828) });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "CompanyId", "CreateAt", "Description", "IsActive", "Name", "Validate" },
                values: new object[] { 2, new DateTime(2023, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9849), "Tabacaria", true, "Rei do Baco", new DateTime(2026, 4, 15, 23, 40, 51, 81, DateTimeKind.Local).AddTicks(9849) });
        }
    }
}
