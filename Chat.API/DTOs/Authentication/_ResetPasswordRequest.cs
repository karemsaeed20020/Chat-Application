namespace Chat.API.DTOs.Authentication
{
    public record _ResetPasswordRequest(string Email, string Code, string NewPassword);
}
