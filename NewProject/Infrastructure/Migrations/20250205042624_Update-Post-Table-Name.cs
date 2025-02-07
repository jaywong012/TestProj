using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPostShare_Accounts_AccountId",
                table: "AccountPostShare");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountPostShare_Post_PostId",
                table: "AccountPostShare");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountPostShare",
                table: "AccountPostShare");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "AccountPostShare",
                newName: "AccountPostShares");

            migrationBuilder.RenameIndex(
                name: "IX_AccountPostShare_PostId",
                table: "AccountPostShares",
                newName: "IX_AccountPostShares_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountPostShare_AccountId",
                table: "AccountPostShares",
                newName: "IX_AccountPostShares_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountPostShares",
                table: "AccountPostShares",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPostShares_Accounts_AccountId",
                table: "AccountPostShares",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPostShares_Posts_PostId",
                table: "AccountPostShares",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPostShares_Accounts_AccountId",
                table: "AccountPostShares");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountPostShares_Posts_PostId",
                table: "AccountPostShares");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountPostShares",
                table: "AccountPostShares");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "AccountPostShares",
                newName: "AccountPostShare");

            migrationBuilder.RenameIndex(
                name: "IX_AccountPostShares_PostId",
                table: "AccountPostShare",
                newName: "IX_AccountPostShare_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountPostShares_AccountId",
                table: "AccountPostShare",
                newName: "IX_AccountPostShare_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountPostShare",
                table: "AccountPostShare",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPostShare_Accounts_AccountId",
                table: "AccountPostShare",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPostShare_Post_PostId",
                table: "AccountPostShare",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
