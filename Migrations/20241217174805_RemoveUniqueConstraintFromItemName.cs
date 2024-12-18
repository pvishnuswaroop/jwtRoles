using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickServeAPP.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromItemName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
        name: "IX_Menus_ItemName",
        table: "Menus"
    );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
        name: "IX_Menus_ItemName",
        table: "Menus",
        column: "ItemName",
        unique: true
    );
        }
    }
}
