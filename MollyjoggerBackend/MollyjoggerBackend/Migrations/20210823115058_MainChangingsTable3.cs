using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MollyjoggerBackend.Migrations
{
    public partial class MainChangingsTable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ShopOfProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ShopOfProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ShopOfProducts");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ShopOfProducts");
        }
    }
}
