using Microsoft.EntityFrameworkCore;
using music_blog_server.Models.Domain;

namespace music_blog_server.Data
{
    public class MusicBlogDbContext : DbContext
    {
        public MusicBlogDbContext(DbContextOptions options) : base(options) 
        {
            
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
    }
}
