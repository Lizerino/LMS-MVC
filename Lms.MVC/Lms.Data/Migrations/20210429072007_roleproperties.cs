using Microsoft.EntityFrameworkCore.Migrations;

namespace Lms.MVC.Data.Migrations
{
    public partial class roleproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_Teacher_CourseId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Teacher_CourseId",
                table: "AspNetUsers",
                newName: "CourseId2");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Teacher_CourseId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_CourseId2");

            migrationBuilder.AddColumn<int>(
                name: "CourseId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CourseId1",
                table: "AspNetUsers",
                column: "CourseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId",
                table: "AspNetUsers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId1",
                table: "AspNetUsers",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId2",
                table: "AspNetUsers",
                column: "CourseId2",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId2",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CourseId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CourseId1",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CourseId2",
                table: "AspNetUsers",
                newName: "Teacher_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CourseId2",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Teacher_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_CourseId",
                table: "AspNetUsers",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Courses_Teacher_CourseId",
                table: "AspNetUsers",
                column: "Teacher_CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
