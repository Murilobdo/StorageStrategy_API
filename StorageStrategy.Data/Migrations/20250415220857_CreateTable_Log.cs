using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogApp",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogApp", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_LogApp_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 4, 15, 19, 8, 56, 380, DateTimeKind.Local).AddTicks(6947), new DateTime(2035, 4, 15, 19, 8, 56, 380, DateTimeKind.Local).AddTicks(6963) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$7voE8M7epnLpNUqZFQXWfQ$4ywrg4eBy2Mat8LPh6Bi1/dk2CNx8hN0OgFFkIHZ16g");

            migrationBuilder.CreateIndex(
                name: "IX_LogApp_EmployeeId",
                table: "LogApp",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogApp");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 3, 3, 20, 11, 31, 913, DateTimeKind.Local).AddTicks(601), new DateTime(2034, 3, 3, 20, 11, 31, 913, DateTimeKind.Local).AddTicks(621) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$IwAFNFrheuYBCOjAdzhlPQ$SbORLXTbw7B4Y2qiIp0KUBDbO4vKrAR0U4ehkc2lnYM");
        }
    }
}
