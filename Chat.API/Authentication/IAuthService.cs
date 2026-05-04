using Chat.API.Abstractions;
using Chat.API.DTOs.Authentication;

namespace Chat.API.Authentication
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(_RegisterRequest request, CancellationToken cancellationToken = default!);
        Task<Result<AuthResponse>> LoginAsync(_LoginRequest request, CancellationToken cancellationToken = default!);
        Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<Result> ConfirmEmailAsync(_ConfirmEmailRequest request);
        Task<Result> ResendConfirmationEmailAsync(_ResendConfirmationEmailRequest request);
        Task<Result> SendResetPasswordAsync(_ForgotPasswordRequest request);
        Task<Result> ResetPasswordAsync(_ResetPasswordRequest request);
    }
}
