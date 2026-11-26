using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateTables_Printing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrintJob",
                columns: table => new
                {
                    PrintJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CommandId = table.Column<int>(type: "int", nullable: false),
                    RequestedByEmployeeId = table.Column<int>(type: "int", nullable: false),
                    PrintStationId = table.Column<int>(type: "int", nullable: true),
                    Copy = table.Column<int>(type: "int", nullable: false),
                    Payload = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintJob", x => x.PrintJobId);
                });

            migrationBuilder.CreateTable(
                name: "PrintStation",
                columns: table => new
                {
                    PrintStationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", nullable: false),
                    PaperWidthMm = table.Column<int>(type: "int", nullable: false),
                    Encoding = table.Column<int>(type: "int", nullable: false),
                    KeyHash = table.Column<byte[]>(type: "varbinary(32)", nullable: false),
                    KeyLastChars = table.Column<string>(type: "nvarchar(4)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AtualizadoPorEmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintStation", x => x.PrintStationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrintJob_CommandId_CreatedAt",
                table: "PrintJob",
                columns: new[] { "CommandId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PrintJob_CompanyId_Status_CreatedAt",
                table: "PrintJob",
                columns: new[] { "CompanyId", "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PrintStation_CompanyId",
                table: "PrintStation",
                column: "CompanyId",
                unique: true,
                filter: "[IsActive] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrintJob");

            migrationBuilder.DropTable(
                name: "PrintStation");
        }
    }
}
