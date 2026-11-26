using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTable_PixCredential : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PixCharge",
                columns: table => new
                {
                    PixChargeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CommandId = table.Column<int>(type: "int", nullable: false),
                    MpPaymentId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrCodeBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiraEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PixCharge", x => x.PixChargeId);
                });

            migrationBuilder.CreateTable(
                name: "PixCredential",
                columns: table => new
                {
                    PixCredentialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    AccessTokenEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessTokenLastDigits = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebhookSecretEncrypted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoPorEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PixCredential", x => x.PixCredentialId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PixCharge_CommandId",
                table: "PixCharge",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_PixCharge_CompanyId",
                table: "PixCharge",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_PixCharge_MpPaymentId",
                table: "PixCharge",
                column: "MpPaymentId",
                unique: true,
                filter: "[MpPaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PixCredential_CompanyId",
                table: "PixCredential",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PixCharge");

            migrationBuilder.DropTable(
                name: "PixCredential");
        }
    }
}
