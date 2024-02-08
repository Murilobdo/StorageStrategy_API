using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class Company : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
