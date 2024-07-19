using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeStuff.Repository.Migrations
{
    /// <inheritdoc />
    public partial class update_token : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tokens_Accounts_AccountId",
                table: "Tokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tokens",
                table: "Tokens");

            migrationBuilder.RenameTable(
                name: "Tokens",
                newName: "Token");

            migrationBuilder.RenameIndex(
                name: "IX_Tokens_AccountId",
                table: "Token",
                newName: "IX_Token_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Token",
                table: "Token",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Token_Accounts_AccountId",
                table: "Token",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Token_Accounts_AccountId",
                table: "Token");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Token",
                table: "Token");

            migrationBuilder.RenameTable(
                name: "Token",
                newName: "Tokens");

            migrationBuilder.RenameIndex(
                name: "IX_Token_AccountId",
                table: "Tokens",
                newName: "IX_Tokens_AccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tokens",
                table: "Tokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tokens_Accounts_AccountId",
                table: "Tokens",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
