using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableClientCompanyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Client",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 6, 22, 14, 42, 26, 240, DateTimeKind.Local).AddTicks(9827), new DateTime(2035, 6, 22, 14, 42, 26, 240, DateTimeKind.Local).AddTicks(9845) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$R7+Jz6t7OtWIvYCOaGylRw$Xev79b3VgRA6fLnE51JOsm3dvugWq+kpG7qITOA1k9E");

            migrationBuilder.CreateIndex(
                name: "IX_Client_CompanyId",
                table: "Client",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Company_CompanyId",
                table: "Client",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Company_CompanyId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_CompanyId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Client");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 6, 22, 14, 38, 15, 645, DateTimeKind.Local).AddTicks(5987), new DateTime(2035, 6, 22, 14, 38, 15, 645, DateTimeKind.Local).AddTicks(6004) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$IxecMrZ3I2PjNlU0M4pFLQ$dJes/SDbQkIjBc2lYp9zJ79JZ+Pcevl6+Ydj7yEfFCc");
        }
    }
}
