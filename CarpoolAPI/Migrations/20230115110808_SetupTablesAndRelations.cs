using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarpoolAPI.Migrations
{
    public partial class SetupTablesAndRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DriverId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    carModel = table.Column<string>(type: "TEXT", nullable: false),
                    seatCount = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    startTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    endTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    day = table.Column<int>(type: "INTEGER", nullable: false),
                    startLocation = table.Column<string>(type: "TEXT", nullable: false),
                    endLocation = table.Column<string>(type: "TEXT", nullable: false),
                    DriverId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rides_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RideUser",
                columns: table => new
                {
                    RidesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideUser", x => new { x.RidesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_RideUser_Rides_RidesId",
                        column: x => x.RidesId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DriverId",
                table: "Users",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                table: "Rides",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_RideUser_UsersId",
                table: "RideUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Drivers_DriverId",
                table: "Users",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Drivers_DriverId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "RideUser");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Users_DriverId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");
        }
    }
}
