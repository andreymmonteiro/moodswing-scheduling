using Microsoft.EntityFrameworkCore.Migrations;

namespace Moodswing.Data.Migrations
{
    public partial class person_id_fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "person_Id",
                table: "schedule",
                newName: "person_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "person_id",
                table: "schedule",
                newName: "person_Id");
        }
    }
}
