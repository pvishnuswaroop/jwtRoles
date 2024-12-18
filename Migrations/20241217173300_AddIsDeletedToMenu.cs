using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickServeAPP.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Menus");
        }
    }
}
