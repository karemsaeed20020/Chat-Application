using Chat.API.Abstractions;
using Chat.API.DTOs.Account;
using Chat.API.Entities;
using Chat.API.Errors;
using Chat.API.Mapping;
using Microsoft.AspNetCore.Identity;

namespace Chat.API.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result> ActivateAsync(string userId, CancellationToken canellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(UserError.UserNotFound);

            if (user.IsDeleted != true)
                return Result.Failure(AccountErrors.AccountActivated);

            user.IsDeleted = false;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }

        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request, CancellationToken canellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(UserError.UserNotFound);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword); //already handeled (currentReqPass ==  currentDbPass)
            if (result.Succeeded)
                return Result.Success();
            return Result.Failure(UserError.ChangePasswordFailed);
        }

        public async Task<Result> DeactivateAsync(string userId, CancellationToken canellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(UserError.UserNotFound);

            if (user.IsDeleted != false)
                return Result.Failure(AccountErrors.AccountDeactivated);

            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(string userId, CancellationToken canellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure(UserError.UserNotFound);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }
            return Result.Success();
        }

        public async Task<Result<UserProfileResponse>> OtherUserProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure<UserProfileResponse>(UserError.UserNotFound);

            var response = user.MapToUserProfileResponse();
            return Result.Success(response);
        }

        public async Task<Result<UserProfileRequest>> UpdateUserProfileAsync(string userId, UserProfileRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure<UserProfileRequest>(UserError.UserNotFound);

            var updatedUser = request.MapToUser(user);

            var result = await _userManager.UpdateAsync(updatedUser);
            if (result.Succeeded)
                return Result.Success(request);

            return Result.Failure<UserProfileRequest>(UserError.UpdateFailed);
        }

        public async Task<Result<UserProfileResponse>> UserProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Failure<UserProfileResponse>(UserError.UserNotFound);

            var response = user.MapToUserProfileResponse();
            return Result.Success(response);
        }
    }
}
