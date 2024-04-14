using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RiverBooks.Users.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_UserAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStreetAddress_AspNetUsers_ApplicationUserId",
                schema: "Users",
                table: "UserStreetAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStreetAddress",
                schema: "Users",
                table: "UserStreetAddress");

            migrationBuilder.RenameTable(
                name: "UserStreetAddress",
                schema: "Users",
                newName: "UserStreetAddresses",
                newSchema: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_UserStreetAddress_ApplicationUserId",
                schema: "Users",
                table: "UserStreetAddresses",
                newName: "IX_UserStreetAddresses_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStreetAddresses",
                schema: "Users",
                table: "UserStreetAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStreetAddresses_AspNetUsers_ApplicationUserId",
                schema: "Users",
                table: "UserStreetAddresses",
                column: "ApplicationUserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStreetAddresses_AspNetUsers_ApplicationUserId",
                schema: "Users",
                table: "UserStreetAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStreetAddresses",
                schema: "Users",
                table: "UserStreetAddresses");

            migrationBuilder.RenameTable(
                name: "UserStreetAddresses",
                schema: "Users",
                newName: "UserStreetAddress",
                newSchema: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_UserStreetAddresses_ApplicationUserId",
                schema: "Users",
                table: "UserStreetAddress",
                newName: "IX_UserStreetAddress_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStreetAddress",
                schema: "Users",
                table: "UserStreetAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStreetAddress_AspNetUsers_ApplicationUserId",
                schema: "Users",
                table: "UserStreetAddress",
                column: "ApplicationUserId",
                principalSchema: "Users",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
