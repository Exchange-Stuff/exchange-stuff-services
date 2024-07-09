using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExchangeStuff.Repository.Migrations
{
    /// <inheritdoc />
    public partial class chat_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_BoxChats_BoxChatId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageChats_Accounts_UserId",
                table: "MessageChats");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageChats_BoxChats_BoxChatId",
                table: "MessageChats");

            migrationBuilder.DropTable(
                name: "ParticipantChats");

            migrationBuilder.DropTable(
                name: "BoxChats");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_BoxChatId",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageChats",
                table: "MessageChats");

            migrationBuilder.DropColumn(
                name: "BoxChatId",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "MessageChats",
                newName: "MessageChat");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MessageChat",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "BoxChatId",
                table: "MessageChat",
                newName: "GroupChatId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChats_UserId",
                table: "MessageChat",
                newName: "IX_MessageChat_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChats_BoxChatId",
                table: "MessageChat",
                newName: "IX_MessageChat_GroupChatId");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeSend",
                table: "MessageChat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageChat",
                table: "MessageChat",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GroupChat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChat_Accounts_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GroupChat_Accounts_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChat_ReceiverId",
                table: "GroupChat",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChat_SenderId",
                table: "GroupChat",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChat_Accounts_SenderId",
                table: "MessageChat",
                column: "SenderId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChat_GroupChat_GroupChatId",
                table: "MessageChat",
                column: "GroupChatId",
                principalTable: "GroupChat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageChat_Accounts_SenderId",
                table: "MessageChat");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageChat_GroupChat_GroupChatId",
                table: "MessageChat");

            migrationBuilder.DropTable(
                name: "GroupChat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MessageChat",
                table: "MessageChat");

            migrationBuilder.DropColumn(
                name: "TimeSend",
                table: "MessageChat");

            migrationBuilder.RenameTable(
                name: "MessageChat",
                newName: "MessageChats");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "MessageChats",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "GroupChatId",
                table: "MessageChats",
                newName: "BoxChatId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChat_SenderId",
                table: "MessageChats",
                newName: "IX_MessageChats_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageChat_GroupChatId",
                table: "MessageChats",
                newName: "IX_MessageChats_BoxChatId");

            migrationBuilder.AddColumn<Guid>(
                name: "BoxChatId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MessageChats",
                table: "MessageChats",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BoxChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxChats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParticipantChats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoxChatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChats_Accounts_UserId",
                table: "MessageChats",
                column: "UserId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageChats_BoxChats_BoxChatId",
                table: "MessageChats",
                column: "BoxChatId",
                principalTable: "BoxChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
