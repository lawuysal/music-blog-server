using Microsoft.AspNetCore.Identity;

namespace music_blog_server.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user);
    }
}
