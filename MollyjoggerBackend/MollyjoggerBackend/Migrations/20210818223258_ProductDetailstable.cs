using Microsoft.EntityFrameworkCore.Migrations;

namespace MollyjoggerBackend.Migrations
{
    public partial class ProductDetailstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smallimg1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smallimg2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smallimg3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Smallimg4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShopOfProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDetails_ShopOfProducts_ShopOfProductsId",
                        column: x => x.ShopOfProductsId,
                        principalTable: "ShopOfProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDetails_ShopOfProductsId",
                table: "ProductDetails",
                column: "ShopOfProductsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductDetails");
        }
    }
}
