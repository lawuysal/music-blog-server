using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_blog_server.Data;
using music_blog_server.Models.Domain;
using music_blog_server.Models.DTO;
using music_blog_server.Repositories;

namespace music_blog_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleImagesController : ControllerBase
    {
        private readonly IArticleImageRepository imageRepsitory;
        private readonly MusicBlogDbContext dbContext;

        public ArticleImagesController(IArticleImageRepository imageRepsitory, MusicBlogDbContext dbContext)
        {
            this.imageRepsitory = imageRepsitory;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArticleImages()
        {
            var articleImages = await dbContext.ArticleImages.ToListAsync();

            var articleImagesDto = new List<ArticleImageDto>();

            foreach (var image in articleImages)
            {

                articleImagesDto.Add(new ArticleImageDto
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    FileExtension = image.FileExtension,
                    FilePath = image.FilePath,
                    FileSizeInBytes = image.FileSizeInBytes,
                    FileCreatedAt = image.FileCreatedAt,
                });
            }

            return Ok(articleImagesDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetArticleImageById([FromRoute] Guid id)
        {
            var articleImage = await dbContext.ArticleImages.FindAsync(id);

            if(articleImage == null)
            {
                return NotFound();
            }

            return Ok(articleImage);
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ArticleImageUploadRequestDto request)
        {
            var imageDomainModel = new ArticleImage
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName.ToLower().Trim().Replace(" ", "-"),
                FileCreatedAt = DateTime.Now,
            };

            await imageRepsitory.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteArticleImage([FromRoute] Guid id)
        {
            var articleImage = await dbContext.ArticleImages.FindAsync(id);

            if (articleImage == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), articleImage.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            dbContext.ArticleImages.Remove(articleImage);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
