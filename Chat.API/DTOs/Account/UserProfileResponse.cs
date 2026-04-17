namespace Chat.API.DTOs.Account
{
    public record UserProfileResponse(string FirstName, string LastName, string? PhoneNumber, string? Avatar, string? Bio);
}
