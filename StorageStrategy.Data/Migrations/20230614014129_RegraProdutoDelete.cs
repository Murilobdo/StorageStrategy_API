using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegraProdutoDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "CategoryEntityCategoryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 6, 13, 22, 41, 28, 126, DateTimeKind.Local).AddTicks(4879), new DateTime(2033, 6, 13, 22, 41, 28, 126, DateTimeKind.Local).AddTicks(4893) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$/jAa5EL20awStnPy7h6XdQ$YSAl/9GJjD0a6vfCbe8fEgetJbCxRpu9tK+QYiydaA4");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryEntityCategoryId",
                table: "Product",
                column: "CategoryEntityCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryEntityCategoryId",
                table: "Product",
                column: "CategoryEntityCategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryEntityCategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryEntityCategoryId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_CategoryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "CategoryEntityCategoryId",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "Company",
                keyColumn: "CompanyId",
                keyValue: 1,
                columns: new[] { "CreateAt", "Validate" },
                values: new object[] { new DateTime(2023, 5, 30, 18, 49, 7, 900, DateTimeKind.Local).AddTicks(8818), new DateTime(2033, 5, 30, 18, 49, 7, 900, DateTimeKind.Local).AddTicks(8835) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$A66BQt0ZV7pbBJ4ARD6BSQ$oMnuj8ImqYFdv6cFf38rf1S3GPR/vzcAcqXBe3oTGcE");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
