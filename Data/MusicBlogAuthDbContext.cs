using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace music_blog_server.Data
{
    public class MusicBlogAuthDbContext : IdentityDbContext
    {
        public MusicBlogAuthDbContext(DbContextOptions<MusicBlogAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRoleId = "439B3075-FE5A-49C6-995D-19A111E0B588";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                }
            };

            builder.Entity<IdentityRole>().HasData(roles); 
        }
    }
}
