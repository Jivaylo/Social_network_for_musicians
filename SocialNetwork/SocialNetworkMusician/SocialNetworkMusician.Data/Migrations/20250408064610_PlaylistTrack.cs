using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkMusician.Data.Migrations
{
    public partial class PlaylistTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_MusicTracks_TracksId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistsId",
                table: "PlaylistTracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks");

            migrationBuilder.RenameColumn(
                name: "TracksId",
                table: "PlaylistTracks",
                newName: "PlaylistId");

            migrationBuilder.RenameColumn(
                name: "PlaylistsId",
                table: "PlaylistTracks",
                newName: "MusicTrackId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistTracks_TracksId",
                table: "PlaylistTracks",
                newName: "IX_PlaylistTracks_PlaylistId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PlaylistTracks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MusicTrackId",
                table: "Playlists",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistTracks_MusicTrackId",
                table: "PlaylistTracks",
                column: "MusicTrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_MusicTrackId",
                table: "Playlists",
                column: "MusicTrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_MusicTracks_MusicTrackId",
                table: "Playlists",
                column: "MusicTrackId",
                principalTable: "MusicTracks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_MusicTracks_MusicTrackId",
                table: "PlaylistTracks",
                column: "MusicTrackId",
                principalTable: "MusicTracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistId",
                table: "PlaylistTracks",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_MusicTracks_MusicTrackId",
                table: "Playlists");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_MusicTracks_MusicTrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistId",
                table: "PlaylistTracks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks");

            migrationBuilder.DropIndex(
                name: "IX_PlaylistTracks_MusicTrackId",
                table: "PlaylistTracks");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_MusicTrackId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlaylistTracks");

            migrationBuilder.DropColumn(
                name: "MusicTrackId",
                table: "Playlists");

            migrationBuilder.RenameColumn(
                name: "PlaylistId",
                table: "PlaylistTracks",
                newName: "TracksId");

            migrationBuilder.RenameColumn(
                name: "MusicTrackId",
                table: "PlaylistTracks",
                newName: "PlaylistsId");

            migrationBuilder.RenameIndex(
                name: "IX_PlaylistTracks_PlaylistId",
                table: "PlaylistTracks",
                newName: "IX_PlaylistTracks_TracksId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaylistTracks",
                table: "PlaylistTracks",
                columns: new[] { "PlaylistsId", "TracksId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_MusicTracks_TracksId",
                table: "PlaylistTracks",
                column: "TracksId",
                principalTable: "MusicTracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistTracks_Playlists_PlaylistsId",
                table: "PlaylistTracks",
                column: "PlaylistsId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
