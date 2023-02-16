using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTableExpensesExpensesType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpensesTypeEntity",
                columns: table => new
                {
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesTypeEntity", x => x.ExpenseTypeId);
                    table.ForeignKey(
                        name: "FK_ExpensesTypeEntity_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpensesEntity",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpensesTypeId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpensesEntity", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_ExpensesEntity_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpensesEntity_ExpensesTypeEntity_ExpensesTypeId",
                        column: x => x.ExpensesTypeId,
                        principalTable: "ExpensesTypeEntity",
                        principalColumn: "ExpenseTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 2, 15, 20, 36, 57, 606, DateTimeKind.Local).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 2, 15, 20, 36, 57, 606, DateTimeKind.Local).AddTicks(6587));

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesEntity_CompanyId",
                table: "ExpensesEntity",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesEntity_ExpensesTypeId",
                table: "ExpensesEntity",
                column: "ExpensesTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpensesTypeEntity_CompanyId",
                table: "ExpensesTypeEntity",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpensesEntity");

            migrationBuilder.DropTable(
                name: "ExpensesTypeEntity");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2023, 1, 26, 21, 39, 50, 780, DateTimeKind.Local).AddTicks(808));

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2023, 1, 26, 21, 39, 50, 780, DateTimeKind.Local).AddTicks(845));
        }
    }
}
