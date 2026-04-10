namespace Chat.API.DTOs.Authentication
{
    public record _RegisterRequest
    (
        string Email,
        string FirstName,
        string LastName,
        string Password
    );
}
