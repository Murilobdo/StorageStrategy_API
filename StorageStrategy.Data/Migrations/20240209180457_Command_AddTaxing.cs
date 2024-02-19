using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandAddTaxing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalTaxing",
                table: "Command",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 9, 15, 4, 56, 381, DateTimeKind.Local).AddTicks(7968), new DateTime(2034, 2, 9, 15, 4, 56, 381, DateTimeKind.Local).AddTicks(7983) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$Iz7qfapW3a6ZMft6+5kyMg$AEmUtvp1guS6cLzDGqCXy5u8VyisFaEVDP4Z/R/30tQ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTaxing",
                table: "Command");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 2, 8, 13, 27, 53, 316, DateTimeKind.Local).AddTicks(4090), new DateTime(2034, 2, 8, 13, 27, 53, 316, DateTimeKind.Local).AddTicks(4106) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$RQc93gTReMKBRwqJYge6KQ$R7UhOJ70ULjwo5UcYdiEF02ot4+zOjDJ7RIv8RaTBhE");
        }
    }
}
