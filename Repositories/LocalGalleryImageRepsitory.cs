using music_blog_server.Data;
using music_blog_server.Models.Domain;

namespace music_blog_server.Repositories
{
    public class LocalGalleryImageRepsitory : IGalleryImageRepsitory
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly MusicBlogDbContext dbContext;

        public LocalGalleryImageRepsitory(IWebHostEnvironment webHostEnvironment, MusicBlogDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.dbContext = dbContext;
        }
        public async Task<GalleryImage> Upload(GalleryImage image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", "Gallery", $"{image.FileName}{image.FileExtension}");

            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var filePath = $"Images/Gallery/{image.FileName}{image.FileExtension}";
            
            image.FilePath = filePath;

            await dbContext.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;

        }
    }
}
