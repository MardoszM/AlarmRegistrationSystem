using Microsoft.EntityFrameworkCore.Migrations;

namespace AlarmRegistrationSystem.Migrations
{
    public partial class NotificationUpdate21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MachineUniqueID",
                table: "Notifications",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MachineUniqueID",
                table: "Notifications",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
