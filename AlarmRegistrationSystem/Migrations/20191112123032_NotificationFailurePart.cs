using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AlarmRegistrationSystem.Migrations
{
    public partial class NotificationFailurePart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Descriptions",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationEs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    ESId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subassemblies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subassemblies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationEs");

            migrationBuilder.DropTable(
                name: "Subassemblies");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Descriptions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000);
        }
    }
}
