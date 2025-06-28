using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientTableCommand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Command",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Command_ClientId",
                table: "Command",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Command_Client_ClientId",
                table: "Command",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Command_Client_ClientId",
                table: "Command");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Command_ClientId",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Command");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2025, 4, 18, 12, 23, 35, 324, DateTimeKind.Local).AddTicks(4758), new DateTime(2035, 4, 18, 12, 23, 35, 324, DateTimeKind.Local).AddTicks(4776) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$5Nfybd+3gRJIwQgJVtLMCA$CWky2M9JpotQ5OOcaucvaIfw6UNMkGYOovy7MF/jccQ");
        }
    }
}
