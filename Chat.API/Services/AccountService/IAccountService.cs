using Chat.API.Abstractions;
using Chat.API.DTOs.Account;

namespace Chat.API.Services.AccountService
{
    public interface IAccountService
    {
        Task<Result<UserProfileResponse>> UserProfileAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<UserProfileResponse>> OtherUserProfileAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<UserProfileRequest>> UpdateUserProfileAsync(string userId, UserProfileRequest request, CancellationToken cancellationToken = default);
        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken canellationToken = default);

        Task<Result> DeactivateAsync(string userId, CancellationToken canellationToken = default);

        Task<Result> ActivateAsync(string userId, CancellationToken canellationToken = default);

        Task<Result> DeleteAsync(string userId, CancellationToken canellationToken = default);
    }
}
