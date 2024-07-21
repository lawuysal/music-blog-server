using Microsoft.EntityFrameworkCore;

namespace music_blog_server.Data
{
    public class MusicBlogDbContext : DbContext
    {
        public MusicBlogDbContext(DbContextOptions options) : base(options) { 
        }
    }
}
