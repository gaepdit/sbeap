using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbeap.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddSicCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SicCodeId",
                table: "Customers",
                type: "nvarchar(4)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SicCodes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SicCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_SicCodeId",
                table: "Customers",
                column: "SicCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_SicCodes_SicCodeId",
                table: "Customers",
                column: "SicCodeId",
                principalTable: "SicCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_SicCodes_SicCodeId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "SicCodes");

            migrationBuilder.DropIndex(
                name: "IX_Customers_SicCodeId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SicCodeId",
                table: "Customers");
        }
    }
}
