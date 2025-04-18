using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTablePayments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Command");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandId = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payment_Command_CommandId",
                        column: x => x.CommandId,
                        principalTable: "Command",
                        principalColumn: "CommandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 4, 17, 21, 55, 28, 264, DateTimeKind.Local).AddTicks(8598), new DateTime(2035, 4, 17, 21, 55, 28, 264, DateTimeKind.Local).AddTicks(8613) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$shu4UKDQIaCKvIPdVO+Hng$HpDqHonobK+KdzoD3lWzapEvuRbfQbH/tlv+X0FzY54");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CommandId",
                table: "Payment",
                column: "CommandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "Payment",
                table: "Command",
                type: "int",
                nullable: true);

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
        }
    }
}
