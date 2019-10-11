using Microsoft.EntityFrameworkCore.Migrations;

namespace AlarmRegistrationSystem.Migrations
{
    public partial class NotificationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "MachineID",
                table: "Notifications",
                newName: "MachineUniqueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MachineUniqueID",
                table: "Notifications",
                newName: "MachineID");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Notifications",
                nullable: true);
        }
    }
}
