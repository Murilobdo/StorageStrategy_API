using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeCreateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Comission = table.Column<int>(type: "int", nullable: false),
                    JobRole = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2022, 12, 26, 16, 45, 12, 831, DateTimeKind.Local).AddTicks(7673));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2022, 12, 26, 16, 45, 12, 831, DateTimeKind.Local).AddTicks(7694));

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                table: "Employee",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2022, 12, 23, 1, 20, 28, 107, DateTimeKind.Local).AddTicks(4202));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2022, 12, 23, 1, 20, 28, 107, DateTimeKind.Local).AddTicks(4224));
        }
    }
}
