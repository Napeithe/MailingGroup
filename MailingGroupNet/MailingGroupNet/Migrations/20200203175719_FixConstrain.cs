using Microsoft.EntityFrameworkCore.Migrations;

namespace MailingGroupNet.Migrations
{
    public partial class FixConstrain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MailingGroups_AspNetUsers_UserId",
                table: "MailingGroups");

            migrationBuilder.DropIndex(
                name: "IX_MailingGroups_Name",
                table: "MailingGroups");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MailingGroups",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MailingGroups_AspNetUsers_UserId",
                table: "MailingGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MailingGroups_AspNetUsers_UserId",
                table: "MailingGroups");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MailingGroups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_MailingGroups_Name",
                table: "MailingGroups",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MailingGroups_AspNetUsers_UserId",
                table: "MailingGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
