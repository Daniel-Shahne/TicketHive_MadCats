using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TicketHive_MadCats.Server.MigrationsEventTicket
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketPrice = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageSrcs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxTickets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventModelId",
                        column: x => x.EventModelId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Date", "EventType", "ImageSrcs", "Location", "MaxTickets", "Name", "TicketPrice" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 9, 13, 39, 43, 815, DateTimeKind.Local).AddTicks(3504), "TestEventType1", "[\"/images/event images/image 1.avif\",\"/images/event images/image 2.avif\"]", "TestEvent1Location", 5, "TestEvent1", 100 },
                    { 2, new DateTime(2023, 4, 9, 13, 39, 43, 815, DateTimeKind.Local).AddTicks(3559), "TestEventType2", "[\"/images/event images/image 3.avif\",\"/images/event images/image 4.avif\"]", "TestEvent2Location", 2, "TestEvent2", 100 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "EventModelId", "Username" },
                values: new object[,]
                {
                    { 1, 1, "admin" },
                    { 2, 1, "admin" },
                    { 3, 2, "admin" },
                    { 4, 2, "user" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventModelId",
                table: "Tickets",
                column: "EventModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
