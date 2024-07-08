using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeStuff.Repository.Migrations
{
    /// <inheritdoc />
    public partial class chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BoxChatId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoxChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageChats_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageChats_BoxChats_BoxChatId",
                        column: x => x.BoxChatId,
                        principalTable: "BoxChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantChats_Accounts_UserId",
                        column: x => x.UserId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParticipantChats_BoxChats_BoxChatId",
                        column: x => x.BoxChatId,
                        principalTable: "BoxChats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_BoxChatId",
                table: "Accounts",
                column: "BoxChatId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageChats_BoxChatId",
                table: "MessageChats",
                column: "BoxChatId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageChats_UserId",
                table: "MessageChats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantChats_BoxChatId",
                table: "ParticipantChats",
                column: "BoxChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantChats_UserId",
                table: "ParticipantChats",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_BoxChats_BoxChatId",
                table: "Accounts",
                column: "BoxChatId",
                principalTable: "BoxChats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_BoxChats_BoxChatId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "MessageChats");

            migrationBuilder.DropTable(
                name: "ParticipantChats");

            migrationBuilder.DropTable(
                name: "BoxChats");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BoxChatId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "BoxChatId",
                table: "Accounts");
        }
    }
}
