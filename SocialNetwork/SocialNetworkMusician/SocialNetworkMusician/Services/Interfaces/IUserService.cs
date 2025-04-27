using SocialNetworkMusician.Data.Data;
using SocialNetworkMusician.Models;
using System.Security.Claims;

namespace SocialNetworkMusician.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileViewModel?> GetUserProfileAsync(string userId, ClaimsPrincipal currentUser);
        Task FollowUserAsync(string currentUserId, string targetUserId);
        Task UnfollowUserAsync(string currentUserId, string targetUserId);
        Task<List<UserProfileViewModel>> GetUsersListAsync(string currentUserId, string? search);
        EditProfileViewModel GetEditProfileModel(ApplicationUser user);
        Task UpdateUserProfileAsync(ApplicationUser user, EditProfileViewModel model);
    }
}
