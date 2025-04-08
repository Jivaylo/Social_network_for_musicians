using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetworkMusician.Data.Migrations
{
    public partial class AddPlayCountToMusicTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlayCount",
                table: "MusicTracks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayCount",
                table: "MusicTracks");
        }
    }
}
