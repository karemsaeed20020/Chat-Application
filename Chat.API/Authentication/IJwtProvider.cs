using Chat.API.Entities;

namespace Chat.API.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expireIn) GenerateToken(User user, IEnumerable<string> roles);
    }
}
