using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeStuff.Repository.Migrations
{
    /// <inheritdoc />
    public partial class update_banreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBanReports_Accounts_UserId",
                table: "UserBanReports");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBanReports_BanReasons_BanReasonId",
                table: "UserBanReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBanReports",
                table: "UserBanReports");

            migrationBuilder.RenameTable(
                name: "UserBanReports",
                newName: "UserBanReport");

            migrationBuilder.RenameIndex(
                name: "IX_UserBanReports_UserId",
                table: "UserBanReport",
                newName: "IX_UserBanReport_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBanReports_BanReasonId",
                table: "UserBanReport",
                newName: "IX_UserBanReport_BanReasonId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ProductBanReports",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserCreateId",
                table: "UserBanReport",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBanReport",
                table: "UserBanReport",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBanReports_UserId",
                table: "ProductBanReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBanReport_UserCreateId",
                table: "UserBanReport",
                column: "UserCreateId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBanReports_Accounts_UserId",
                table: "ProductBanReports",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanReport_Accounts_UserCreateId",
                table: "UserBanReport",
                column: "UserCreateId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanReport_Accounts_UserId",
                table: "UserBanReport",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanReport_BanReasons_BanReasonId",
                table: "UserBanReport",
                column: "BanReasonId",
                principalTable: "BanReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBanReports_Accounts_UserId",
                table: "ProductBanReports");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBanReport_Accounts_UserCreateId",
                table: "UserBanReport");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBanReport_Accounts_UserId",
                table: "UserBanReport");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBanReport_BanReasons_BanReasonId",
                table: "UserBanReport");

            migrationBuilder.DropIndex(
                name: "IX_ProductBanReports_UserId",
                table: "ProductBanReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserBanReport",
                table: "UserBanReport");

            migrationBuilder.DropIndex(
                name: "IX_UserBanReport_UserCreateId",
                table: "UserBanReport");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductBanReports");

            migrationBuilder.DropColumn(
                name: "UserCreateId",
                table: "UserBanReport");

            migrationBuilder.RenameTable(
                name: "UserBanReport",
                newName: "UserBanReports");

            migrationBuilder.RenameIndex(
                name: "IX_UserBanReport_UserId",
                table: "UserBanReports",
                newName: "IX_UserBanReports_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBanReport_BanReasonId",
                table: "UserBanReports",
                newName: "IX_UserBanReports_BanReasonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserBanReports",
                table: "UserBanReports",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanReports_Accounts_UserId",
                table: "UserBanReports",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBanReports_BanReasons_BanReasonId",
                table: "UserBanReports",
                column: "BanReasonId",
                principalTable: "BanReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
