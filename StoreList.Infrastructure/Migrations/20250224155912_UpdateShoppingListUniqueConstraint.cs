using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreList.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShoppingListUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_Name",
                table: "ShoppingLists");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_Name_UserId",
                table: "ShoppingLists",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingLists_Name_UserId",
                table: "ShoppingLists");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingLists_Name",
                table: "ShoppingLists",
                column: "Name",
                unique: true);
        }
    }
}
