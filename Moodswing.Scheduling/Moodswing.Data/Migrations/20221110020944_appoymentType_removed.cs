using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Moodswing.Data.Migrations
{
    public partial class appoymentType_removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_schedule_appointment_type_appointment_type_id",
                table: "schedule");

            migrationBuilder.DropTable(
                name: "appointment_type");

            migrationBuilder.DropIndex(
                name: "IX_schedule_appointment_type_id",
                table: "schedule");

            migrationBuilder.DropColumn(
                name: "appointment_type_id",
                table: "schedule");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "appointment_type_id",
                table: "schedule",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "appointment_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    consultation_time = table.Column<int>(type: "integer", nullable: false),
                    create_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    update_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_appointment_type", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedule_appointment_type_id",
                table: "schedule",
                column: "appointment_type_id");

            migrationBuilder.AddForeignKey(
                name: "FK_schedule_appointment_type_appointment_type_id",
                table: "schedule",
                column: "appointment_type_id",
                principalTable: "appointment_type",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
