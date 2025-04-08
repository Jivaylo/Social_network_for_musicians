using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkMusician.Data.Migrations
{
    public partial class AddMusicTracksAndPlaylistsToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "MusicTracks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusicTracks_ApplicationUserId",
                table: "MusicTracks",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MusicTracks_AspNetUsers_ApplicationUserId",
                table: "MusicTracks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MusicTracks_AspNetUsers_ApplicationUserId",
                table: "MusicTracks");

            migrationBuilder.DropIndex(
                name: "IX_MusicTracks_ApplicationUserId",
                table: "MusicTracks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "MusicTracks");
        }
    }
}
