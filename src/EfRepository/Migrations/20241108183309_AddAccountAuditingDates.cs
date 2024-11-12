using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sbeap.EfRepository.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountAuditingDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AzureAdObjectId",
                table: "AspNetUsers",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountCreatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AccountUpdatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "MostRecentLogin",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProfileUpdatedAt",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountCreatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountUpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MostRecentLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileUpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AzureAdObjectId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldMaxLength: 36,
                oldNullable: true);
        }
    }
}
