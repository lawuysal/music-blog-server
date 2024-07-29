using music_blog_server.Data;
using music_blog_server.Models.Domain;

namespace music_blog_server.Repositories
{
    public class LocalArticleImageRepository : IArticleImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly MusicBlogDbContext dbContext;

        public LocalArticleImageRepository(IWebHostEnvironment webHostEnvironment, MusicBlogDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
        }
        public async Task<ArticleImage> Upload(ArticleImage image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", "Article", $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var filePath = $"Images/Article/{image.FileName}{image.FileExtension}";

            image.FilePath = filePath;

            await dbContext.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
