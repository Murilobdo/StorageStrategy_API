using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CompanyAddFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CNPJ",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Company",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "Address", "CNPJ", "CreateAt", "Phone", "Validate" },
                values: new object[] { "", "", new DateTime(2024, 3, 3, 20, 11, 31, 913, DateTimeKind.Local).AddTicks(601), "", new DateTime(2034, 3, 3, 20, 11, 31, 913, DateTimeKind.Local).AddTicks(621) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$IwAFNFrheuYBCOjAdzhlPQ$SbORLXTbw7B4Y2qiIp0KUBDbO4vKrAR0U4ehkc2lnYM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "CNPJ",
                table: "Company");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Company");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 19, 18, 54, 48, 627, DateTimeKind.Local).AddTicks(5791), new DateTime(2034, 2, 19, 18, 54, 48, 627, DateTimeKind.Local).AddTicks(5805) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$i6HmxsHEIynDrrJOIoQKZg$wmLlPAo3N9xgT25IS8WtWdz0rtzmt4ktTpO8CidYhBo");
        }
    }
}
