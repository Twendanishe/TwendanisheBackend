using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Twendanishe.Migrations
{
    public partial class VehicleDestinationRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableCapacity",
                table: "Destinations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Destinations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Destinations_VehicleId",
                table: "Destinations",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Vehicles_VehicleId",
                table: "Destinations",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Vehicles_VehicleId",
                table: "Destinations");

            migrationBuilder.DropIndex(
                name: "IX_Destinations_VehicleId",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "AvailableCapacity",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Destinations");
        }
    }
}
