namespace Chat.API.DTOs.Account
{
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword);
}
