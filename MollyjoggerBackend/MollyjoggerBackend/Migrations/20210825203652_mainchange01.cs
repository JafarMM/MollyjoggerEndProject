using Microsoft.EntityFrameworkCore.Migrations;

namespace MollyjoggerBackend.Migrations
{
    public partial class mainchange01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Categories_CategoryId",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_ShopOfProducts_ShopOfProductsId",
                table: "ProductCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory");

            migrationBuilder.RenameTable(
                name: "ProductCategory",
                newName: "productCategories");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_ShopOfProductsId",
                table: "productCategories",
                newName: "IX_productCategories_ShopOfProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductCategory_CategoryId",
                table: "productCategories",
                newName: "IX_productCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_productCategories",
                table: "productCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_productCategories_Categories_CategoryId",
                table: "productCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_productCategories_ShopOfProducts_ShopOfProductsId",
                table: "productCategories",
                column: "ShopOfProductsId",
                principalTable: "ShopOfProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productCategories_Categories_CategoryId",
                table: "productCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_productCategories_ShopOfProducts_ShopOfProductsId",
                table: "productCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_productCategories",
                table: "productCategories");

            migrationBuilder.RenameTable(
                name: "productCategories",
                newName: "ProductCategory");

            migrationBuilder.RenameIndex(
                name: "IX_productCategories_ShopOfProductsId",
                table: "ProductCategory",
                newName: "IX_ProductCategory_ShopOfProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_productCategories_CategoryId",
                table: "ProductCategory",
                newName: "IX_ProductCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCategory",
                table: "ProductCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Categories_CategoryId",
                table: "ProductCategory",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_ShopOfProducts_ShopOfProductsId",
                table: "ProductCategory",
                column: "ShopOfProductsId",
                principalTable: "ShopOfProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
