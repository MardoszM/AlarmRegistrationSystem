using Microsoft.EntityFrameworkCore.Migrations;

namespace AlarmRegistrationSystem.Migrations
{
    public partial class NotificationFailurePart_vol2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subassemblies",
                table: "Subassemblies");

            migrationBuilder.RenameTable(
                name: "Subassemblies",
                newName: "EmergencySubassemblies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmergencySubassemblies",
                table: "EmergencySubassemblies",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmergencySubassemblies",
                table: "EmergencySubassemblies");

            migrationBuilder.RenameTable(
                name: "EmergencySubassemblies",
                newName: "Subassemblies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subassemblies",
                table: "Subassemblies",
                column: "Id");
        }
    }
}
