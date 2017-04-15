using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace cal.Data.Migrations
{
    public partial class setupcal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "AspNetUserClaims",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "AspNetRoleClaims",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomReservationID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Viewers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    ViewType = table.Column<int>(nullable: false),
                    ViewerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viewers_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Viewers_AspNetUsers_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoomReservation",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    ReservationType = table.Column<int>(nullable: false),
                    RoomID = table.Column<Guid>(nullable: true),
                    Start = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomReservation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RoomReservation_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomReservation_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    EventID = table.Column<Guid>(nullable: true),
                    ReservationType = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_RoomReservation_EventID",
                        column: x => x.EventID,
                        principalTable: "RoomReservation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomReservationID",
                table: "AspNetUsers",
                column: "RoomReservationID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventID",
                table: "Events",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UserId",
                table: "Events",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomReservation_OwnerId",
                table: "RoomReservation",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomReservation_RoomID",
                table: "RoomReservation",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Viewers_OwnerId",
                table: "Viewers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Viewers_ViewerId",
                table: "Viewers",
                column: "ViewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RoomReservation_RoomReservationID",
                table: "AspNetUsers",
                column: "RoomReservationID",
                principalTable: "RoomReservation",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RoomReservation_RoomReservationID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Viewers");

            migrationBuilder.DropTable(
                name: "RoomReservation");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomReservationID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomReservationID",
                table: "AspNetUsers");

            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "AspNetUserClaims",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "AspNetRoleClaims",
            //     nullable: false,
            //     oldClrType: typeof(int))
            //     .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
