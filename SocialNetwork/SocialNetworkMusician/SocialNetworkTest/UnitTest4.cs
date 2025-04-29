/*using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Data;
using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkTest
{
    [TestFixture]
    public class PlaylistsControllerTests
    {
        private PlaylistsController _controller;
        private ApplicationDbContext _dbContext;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ApplicationUser _testUser;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique db every test
                .Options;

            _dbContext = new ApplicationDbContext(options);

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser { Id = "test-user-id", Email = "test@example.com", DisplayName = "Test User" };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _dbContext.Users.Add(_testUser);
            _dbContext.SaveChanges();

            _controller = new PlaylistsController(_dbContext, _userManagerMock.Object);
        }

        [Test]
        public async Task Index_ReturnsView_WithUserPlaylists()
        {
            _dbContext.Playlists.Add(new Playlist { Id = Guid.NewGuid(), Name = "My Playlist", UserId = _testUser.Id });
            _dbContext.SaveChanges();

            var result = await _controller.Index() as ViewResult;
            var model = result.Model as List<PlaylistViewModel>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Count);
        }

        [Test]
        public async Task Create_AddsNewPlaylist()
        {
            var result = await _controller.Create("New Playlist");

            var playlist = _dbContext.Playlists.FirstOrDefault(p => p.Name == "New Playlist");
            Assert.IsNotNull(playlist);
            Assert.AreEqual(_testUser.Id, playlist.UserId);
        }

        [Test]
        public async Task Details_ReturnsPlaylist_WhenFound()
        {
            var playlistId = Guid.NewGuid();
            var playlist = new Playlist { Id = playlistId, Name = "My Playlist", UserId = _testUser.Id };
            _dbContext.Playlists.Add(playlist);
            _dbContext.SaveChanges();

            var result = await _controller.Details(playlistId, null) as ViewResult;
            var model = result.Model as PlaylistViewModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
            Assert.AreEqual(playlistId, model.Id);
        }

        [Test]
        public async Task AddTrack_AddsTrackToPlaylist()
        {
            // Arrange
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Name = "Playlist",
                UserId = _testUser.Id,
                PlaylistTracks = new List<PlaylistTrack>()
            };

            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Track 1",
                FileUrl = "/uploads/test.mp3", // ✅ required field
                UserId = _testUser.Id           // ✅ required field
            };

            await _dbContext.Playlists.AddAsync(playlist);
            await _dbContext.MusicTracks.AddAsync(track);
            await _dbContext.SaveChangesAsync(); // ✅

            // Act
            var result = await _controller.AddTrack(playlist.Id, track.Id) as RedirectToActionResult;

            // Assert
            var playlistAfter = await _dbContext.Playlists.Include(p => p.PlaylistTracks).FirstOrDefaultAsync(p => p.Id == playlist.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);

            Assert.IsNotNull(playlistAfter);
            Assert.AreEqual(1, playlistAfter.PlaylistTracks.Count);
            Assert.AreEqual(track.Id, playlistAfter.PlaylistTracks.First().MusicTrackId);
        }



        [Test]
        public async Task RemoveTrack_RemovesTrackFromPlaylist()
        {
            // Arrange
            var playlist = new Playlist
            {
                Id = Guid.NewGuid(),
                Name = "Playlist",
                UserId = _testUser.Id,
                PlaylistTracks = new List<PlaylistTrack>()
            };

            var track = new MusicTrack
            {
                Id = Guid.NewGuid(),
                Title = "Track 1",
                FileUrl = "/uploads/test.mp3", // ✅ required field
                UserId = _testUser.Id           // ✅ required field
            };

            playlist.PlaylistTracks.Add(new PlaylistTrack
            {
                PlaylistId = playlist.Id,
                MusicTrackId = track.Id
            });

            await _dbContext.Playlists.AddAsync(playlist);
            await _dbContext.MusicTracks.AddAsync(track);
            await _dbContext.SaveChangesAsync(); // ✅

            // Act
            var result = await _controller.RemoveTrack(playlist.Id, track.Id) as RedirectToActionResult;

            // Assert
            var updatedPlaylist = await _dbContext.Playlists.Include(p => p.PlaylistTracks).FirstOrDefaultAsync(p => p.Id == playlist.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Details", result.ActionName);

            Assert.IsNotNull(updatedPlaylist);
            Assert.AreEqual(0, updatedPlaylist.PlaylistTracks.Count); // ✅
        }


    }
}*/
