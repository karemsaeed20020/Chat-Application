using Chat.API.Abstractions;
using Chat.API.DTOs.Authentication;

namespace Chat.API.Authentication
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(_RegisterRequest request, CancellationToken cancellationToken = default!);
        Task<Result<AuthResponse>> LoginAsync(_LoginRequest request, CancellationToken cancellationToken = default!);
    }
}
