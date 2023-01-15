using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolAPI.Migrations
{
    public partial class NoStartEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "endTime",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "startTime",
                table: "Rides");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "endTime",
                table: "Rides",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "startTime",
                table: "Rides",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }
    }
}
