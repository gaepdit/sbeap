using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbeap.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCustomerSicCodeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SicCodes_SicCodeId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "SicCodeId",
                table: "Customers",
                newName: "SicCode");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_SicCodeId",
                table: "Customers",
                newName: "IX_Customers_SicCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SicCodes_SicCode",
                table: "Customers",
                column: "SicCode",
                principalTable: "SicCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SicCodes_SicCode",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "SicCode",
                table: "Customers",
                newName: "SicCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_SicCode",
                table: "Customers",
                newName: "IX_Customers_SicCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SicCodes_SicCodeId",
                table: "Customers",
                column: "SicCodeId",
                principalTable: "SicCodes",
                principalColumn: "Id");
        }
    }
}
