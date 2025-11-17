using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class Command_Product_NewFields_Both : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoDescription",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Command",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BarCode",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PhotoDescription",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Command");

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "CompanyId", "Address", "CNPJ", "CreateAt", "Description", "IsActive", "Name", "Phone", "Validate" },
                values: new object[] { 1, "", "", new DateTime(2025, 10, 23, 18, 55, 10, 738, DateTimeKind.Local).AddTicks(8175), "Admin", true, "Admin Company", "", new DateTime(2035, 10, 23, 18, 55, 10, 738, DateTimeKind.Local).AddTicks(8193) });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "Comission", "CompanyId", "Email", "IsActive", "JobRole", "Name", "PasswordHash" },
                values: new object[] { 1, 0, 1, "murilobdo@admin.com", true, 7, "Murilo Bernardes (Admin)", "$argon2id$v=19$m=65536,t=3,p=1$e5HKd4T+V4TQYHSr70Yt1g$e/8By8dyu1OG6tFzBE6nO15VfjgHscs0xjF8xcDxKvM" });
        }
    }
}
