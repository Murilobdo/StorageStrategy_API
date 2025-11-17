using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class PaymentMethod_INDEX_UNIQUE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_Company_Method",
                table: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_CompanyId_Company_Method",
                table: "PaymentMethod",
                columns: new[] { "CompanyId", "Company", "Method" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaymentMethod_CompanyId_Company_Method",
                table: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethod_Company_Method",
                table: "PaymentMethod",
                columns: new[] { "Company", "Method" },
                unique: true);
        }
    }
}
