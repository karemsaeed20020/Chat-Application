namespace Chat.API.DTOs.Authentication
{
    public record _LoginRequest
    (
        string Email,
        string Password
    );
}
