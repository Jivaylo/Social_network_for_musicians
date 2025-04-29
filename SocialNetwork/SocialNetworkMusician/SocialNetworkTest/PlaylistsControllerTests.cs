using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkMusician.Controllers;
using SocialNetworkMusician.Models;
using SocialNetworkMusician.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using SocialNetworkMusician.Data.Data;

namespace SocialNetworkTest
{
    [TestFixture]
    public class PlaylistsControllerTests
    {
        private PlaylistsController _controller;
        private Mock<IPlaylistsService> _playlistsServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private ApplicationUser _testUser;

        [SetUp]
        public void Setup()
        {
            _playlistsServiceMock = new Mock<IPlaylistsService>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _testUser = new ApplicationUser { Id = "test-user-id", Email = "test@example.com", DisplayName = "Test User" };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_testUser);

            _controller = new PlaylistsController(_playlistsServiceMock.Object, _userManagerMock.Object);
        }

        [Test]
        public async Task Index_ReturnsView_WithUserPlaylists()
        {
            // Arrange
            var playlists = new List<PlaylistViewModel> { new PlaylistViewModel { Id = Guid.NewGuid(), Name = "Test Playlist" } };
            _playlistsServiceMock.Setup(x => x.GetUserPlaylistsAsync(_testUser.Id)).ReturnsAsync(playlists);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<PlaylistViewModel>>(result.Model);
            Assert.That(((List<PlaylistViewModel>)result.Model).Count, Is.EqualTo(1));
        }

        [Test]
        public async Task Create_CreatesPlaylistAndRedirects()
        {
            // Act
            var result = await _controller.Create("New Playlist") as RedirectToActionResult;

            // Assert
            _playlistsServiceMock.Verify(x => x.CreatePlaylistAsync("New Playlist", _testUser.Id), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public async Task Details_ReturnsPlaylist_WhenFound()
        {
            // Arrange
            var playlistId = Guid.NewGuid();
            var playlist = new PlaylistViewModel { Id = playlistId, Name = "My Playlist" };
            _playlistsServiceMock.Setup(x => x.GetPlaylistDetailsAsync(playlistId, null)).ReturnsAsync((playlist, new List<MusicTrack>()));

            // Act
            var result = await _controller.Details(playlistId, null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<PlaylistViewModel>(result.Model);
            Assert.That(((PlaylistViewModel)result.Model).Id, Is.EqualTo(playlistId));
        }

        [Test]
        public async Task AddTrack_AddsTrackAndRedirects()
        {
            // Arrange
            var playlistId = Guid.NewGuid();
            var trackId = Guid.NewGuid();

            // Act
            var result = await _controller.AddTrack(playlistId, trackId) as RedirectToActionResult;

            // Assert
            _playlistsServiceMock.Verify(x => x.AddTrackToPlaylistAsync(playlistId, trackId), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Details"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(playlistId));
        }

        [Test]
        public async Task RemoveTrack_RemovesTrackAndRedirects()
        {
            // Arrange
            var playlistId = Guid.NewGuid();
            var trackId = Guid.NewGuid();

            // Act
            var result = await _controller.RemoveTrack(playlistId, trackId) as RedirectToActionResult;

            // Assert
            _playlistsServiceMock.Verify(x => x.RemoveTrackFromPlaylistAsync(playlistId, trackId), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Details"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(playlistId));
        }

        [Test]
        public async Task Delete_DeletesPlaylistAndRedirects()
        {
            // Arrange
            var playlistId = Guid.NewGuid();

            // Act
            var result = await _controller.Delete(playlistId) as RedirectToActionResult;

            // Assert
            _playlistsServiceMock.Verify(x => x.DeletePlaylistAsync(playlistId, _testUser.Id), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo("Index"));
        }
    }
}
