using Microsoft.EntityFrameworkCore.Migrations;

namespace MailingGroupNet.Migrations
{
    public partial class AddAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailingGroups_UserId",
                table: "MailingGroups");

            migrationBuilder.CreateIndex(
                name: "IX_MailingGroups_UserId",
                table: "MailingGroups",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MailingGroups_UserId",
                table: "MailingGroups");

            migrationBuilder.CreateIndex(
                name: "IX_MailingGroups_UserId",
                table: "MailingGroups",
                column: "UserId",
                unique: true);
        }
    }
}
