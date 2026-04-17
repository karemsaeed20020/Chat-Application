using Chat.API.Const;
using Chat.API.DTOs.Account;
using Chat.API.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet("profile")]
        public async Task<IActionResult> UserProfile(CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _accountService.UserProfileAsync(userId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value()) : NotFound(result);
        }
        [HttpGet("other-profile/{userId}")]
        public async Task<IActionResult> OtherUserProfile([FromRoute] string userId, CancellationToken cancellationToken)
        {
            var result = await _accountService.OtherUserProfileAsync(userId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value()) : NotFound(result);
        }
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileRequest request, CancellationToken cancellationToken)
        {
            var userId = User.GetUserId();
            var result = await _accountService.UpdateUserProfileAsync(userId, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value()) : BadRequest(result);
        }
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request, CancellationToken canellationToken)
        {
            var userId = User.GetUserId();
            var result = await _accountService.ChangePasswordAsync(userId, request, canellationToken);
            return result.IsSuccess ? Ok() : BadRequest(result);
        }

        [HttpPut("activate")]
        public async Task<IActionResult> ActivateAccount(CancellationToken canellationToken)
        {
            var userId = User.GetUserId();
            var result = await _accountService.ActivateAsync(userId, canellationToken);
            return result.IsSuccess ? NoContent() : BadRequest(result);
        }

        [HttpPut("deactivate")]
        public async Task<IActionResult> DeactivateAccount(CancellationToken canellationToken)
        {
            var userId = User.GetUserId();
            var result = await _accountService.DeactivateAsync(userId, canellationToken);
            return result.IsSuccess ? NoContent() : BadRequest(result);
        }

        [HttpDelete("")]
        public async Task<IActionResult> DeleteAccount(CancellationToken canellationToken)
        {
            var userId = User.GetUserId();
            var result = await _accountService.DeleteAsync(userId, canellationToken);
            return result.IsSuccess ? NoContent() : BadRequest(result);
        }
    }
}
