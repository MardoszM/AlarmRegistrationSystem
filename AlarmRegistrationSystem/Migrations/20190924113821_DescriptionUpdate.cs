using Microsoft.EntityFrameworkCore.Migrations;

namespace AlarmRegistrationSystem.Migrations
{
    public partial class DescriptionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Descriptions_Notifications_NotificationID",
                table: "Descriptions");

            migrationBuilder.DropIndex(
                name: "IX_Descriptions_NotificationID",
                table: "Descriptions");

            migrationBuilder.AlterColumn<int>(
                name: "NotificationID",
                table: "Descriptions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "NotificationID",
                table: "Descriptions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Descriptions_NotificationID",
                table: "Descriptions",
                column: "NotificationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Descriptions_Notifications_NotificationID",
                table: "Descriptions",
                column: "NotificationID",
                principalTable: "Notifications",
                principalColumn: "NotificationID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
