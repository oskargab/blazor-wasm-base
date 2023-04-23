using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbRepository.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordAccounts",
                columns: table => new
                {
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<int>(type: "integer", nullable: true),
                    AllowLogin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordAccounts", x => x.ExternalId);
                });

            migrationBuilder.CreateTable(
                name: "GoogleAccounts",
                columns: table => new
                {
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    AllowLogin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleAccounts", x => x.ExternalId);
                });

            migrationBuilder.CreateTable(
                name: "InternalAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InternalAccountId = table.Column<string>(type: "text", nullable: true),
                    GoogleAccountId = table.Column<string>(type: "text", nullable: true),
                    DiscordAccountId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_DiscordAccounts_DiscordAccountId",
                        column: x => x.DiscordAccountId,
                        principalTable: "DiscordAccounts",
                        principalColumn: "ExternalId");
                    table.ForeignKey(
                        name: "FK_Users_GoogleAccounts_GoogleAccountId",
                        column: x => x.GoogleAccountId,
                        principalTable: "GoogleAccounts",
                        principalColumn: "ExternalId");
                    table.ForeignKey(
                        name: "FK_Users_InternalAccounts_InternalAccountId",
                        column: x => x.InternalAccountId,
                        principalTable: "InternalAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DiscordAccountId",
                table: "Users",
                column: "DiscordAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GoogleAccountId",
                table: "Users",
                column: "GoogleAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InternalAccountId",
                table: "Users",
                column: "InternalAccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DiscordAccounts");

            migrationBuilder.DropTable(
                name: "GoogleAccounts");

            migrationBuilder.DropTable(
                name: "InternalAccounts");
        }
    }
}
