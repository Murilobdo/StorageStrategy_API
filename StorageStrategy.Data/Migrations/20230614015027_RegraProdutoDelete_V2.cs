using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StorageStrategy.Data.Migrations
{
    /// <inheritdoc />
    public partial class RegraProdutoDeleteV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_CategoryEntityCategoryId",
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
                values: new object[] { new DateTime(2023, 6, 13, 22, 50, 27, 36, DateTimeKind.Local).AddTicks(1121), new DateTime(2033, 6, 13, 22, 50, 27, 36, DateTimeKind.Local).AddTicks(1138) });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$argon2id$v=19$m=65536,t=3,p=1$tz1kynOTjrTCNkPi8qtvew$4x5/gOjHs7F+pkdn+ALpHIIol9lJchf3I56n53ykuVY");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
