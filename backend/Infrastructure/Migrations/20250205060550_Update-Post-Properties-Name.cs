using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostPropertiesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostUrl",
                table: "Posts",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "PostType",
                table: "Posts",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Posts",
                newName: "PostUrl");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Posts",
                newName: "PostType");
        }
    }
}
