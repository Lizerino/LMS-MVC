using Microsoft.EntityFrameworkCore.Migrations;

namespace Lms.API.Data.Migrations
{
    public partial class changenameofbirthdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Authors",
                newName: "DateOfBirth");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Authors",
                newName: "BirthDate");
        }
    }
}
