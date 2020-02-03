using Microsoft.EntityFrameworkCore.Migrations;

namespace MailingGroupNet.Migrations
{
    public partial class UpdatemailingrGroupKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Email_MailingGroups_MailingGroupId",
                table: "Email");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Email",
                table: "Email");

            migrationBuilder.RenameTable(
                name: "Email",
                newName: "Emails");

            migrationBuilder.RenameIndex(
                name: "IX_Email_MailingGroupId",
                table: "Emails",
                newName: "IX_Emails_MailingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emails",
                table: "Emails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_MailingGroups_MailingGroupId",
                table: "Emails",
                column: "MailingGroupId",
                principalTable: "MailingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_MailingGroups_MailingGroupId",
                table: "Emails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Emails",
                table: "Emails");

            migrationBuilder.RenameTable(
                name: "Emails",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_Emails_MailingGroupId",
                table: "Email",
                newName: "IX_Email_MailingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Email",
                table: "Email",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Email_MailingGroups_MailingGroupId",
                table: "Email",
                column: "MailingGroupId",
                principalTable: "MailingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
