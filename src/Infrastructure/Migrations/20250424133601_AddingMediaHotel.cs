using System.Collections.Generic;
using DealClean.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealClean.Infrastructure.Migrations
{
    public partial class AddingMediaHotel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<HotelMediaInfo>>(
                name: "Media",
                table: "Hotels",
                type: "jsonb",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Media",
                table: "Hotels");
        }
    }
}
