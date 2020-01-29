using Microsoft.EntityFrameworkCore.Migrations;

namespace MailingGroupNet.Migrations
{
    public partial class requiredEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Email",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Email",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
