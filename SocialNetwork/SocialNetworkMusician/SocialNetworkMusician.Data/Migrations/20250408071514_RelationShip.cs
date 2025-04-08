using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkMusician.Data.Migrations
{
    public partial class RelationShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MusicTrackId1",
                table: "PlaylistTracks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_MusicTrackId1",
                table: "PlaylistTracks",
                column: "MusicTrackId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_MusicTracks_MusicTrackId1",
                table: "PlaylistTracks",
                column: "MusicTrackId1",
                principalTable: "MusicTracks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_MusicTracks_MusicTrackId1",
                table: "PlaylistTracks");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTracks_MusicTrackId1",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "MusicTrackId1",
                table: "PlaylistTracks");
        }
    }
}
