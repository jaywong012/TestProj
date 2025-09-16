using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetIndexForTableProductColumnLastSavedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sqlFilePath = Path.Combine(Directory.GetCurrentDirectory(), 
                "..", 
                "Infrastructure", 
                "Scripts", 
                "Commands",
                "Set_Index_For_Product_Name.sql");

            var sql = File.ReadAllText(sqlFilePath);

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
