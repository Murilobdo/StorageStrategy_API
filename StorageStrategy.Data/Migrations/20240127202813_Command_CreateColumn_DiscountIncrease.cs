using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CommandCreateColumnDiscountIncrease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Command",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Increase",
                table: "Command",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2024, 1, 27, 17, 28, 11, 461, DateTimeKind.Local).AddTicks(1989), new DateTime(2034, 1, 27, 17, 28, 11, 461, DateTimeKind.Local).AddTicks(2004) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$DVo7pa7guHnEXwCHc+Txiw$cBRGTK0XNG0ABEl6tbdmBIpiCWirj4eHd+3Z/6t8KFA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "Increase",
                table: "Command");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 12, 19, 20, 46, 54, 985, DateTimeKind.Local).AddTicks(5328), new DateTime(2033, 12, 19, 20, 46, 54, 985, DateTimeKind.Local).AddTicks(5342) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$oTVuIZC1J3lDIQfdYm3Kiw$nUoWazZm9rI7VqlfBZCm7vBQuND/c4t1Up7rtqQscpk");
        }
    }
}
