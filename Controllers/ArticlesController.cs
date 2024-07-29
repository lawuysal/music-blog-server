using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_blog_server.Data;
using music_blog_server.Models.Domain;
using music_blog_server.Models.Dto;
using music_blog_server.Models.DTO;
using music_blog_server.Repositories;
using music_blog_server.Utility;
using System.Globalization;

namespace music_blog_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly MusicBlogDbContext dbContext;
        private readonly IArticleImageRepository articleImageRepository;

        public ArticlesController(MusicBlogDbContext dbContext,  IArticleImageRepository articleImageRepository)
        {
            this.dbContext = dbContext;
            this.articleImageRepository = articleImageRepository;
        }

        // Get all articles
        // GET: api/articles
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await dbContext.Articles.ToListAsync();
            articles = articles.OrderByDescending(x => x.Date).ToList();

            var articlesDto = new List<ArticleDto>();
            foreach (var article in articles)
            {
                articlesDto.Add(new ArticleDto()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Date = article.Date,
                    Content = article.Content,
                    ArticleImageId = article.ArticleImageId,
                    ImageDesc = article.ImageDesc,
                    Tags = article.Tags,
                    CategoryId = article.CategoryId,
                });
            }

            return Ok(articlesDto);
        }

        // Get article by id
        // GET: api/articles/id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetArticleById([FromRoute] Guid id)
        {
            var article = await dbContext.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Date = article.Date,
                Content = article.Content,
                ArticleImageId = article.ArticleImageId,
                ImageDesc = article.ImageDesc,
                Tags = article.Tags,
                CategoryId = article.CategoryId,
            };

            return Ok(articleDto);
        }

        // Create article
        // POST: api/articles
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromForm] ArticleCreateRequestDto articleCreateRequestDto)
        {
            var category = await dbContext.Categories.FindAsync(articleCreateRequestDto.CategoryId);

            if (category == null)
            {
                return BadRequest("Invalid CategoryId: Category not found");
            }

            ArticleImage articleImage;
            if (articleCreateRequestDto.ArticleImageFile != null)
            {
                var imageDomainModel = new ArticleImage
                {
                    File = articleCreateRequestDto.ArticleImageFile,
                    FileExtension = Path.GetExtension(articleCreateRequestDto.ArticleImageFile.FileName),
                    FileSizeInBytes = articleCreateRequestDto.ArticleImageFile.Length,
                    FileName = Path.GetFileNameWithoutExtension(StringExtensions.RemoveNonEnglishChars(articleCreateRequestDto.ArticleImageFile.FileName)),
                    FileCreatedAt = DateTime.Now,
                };

                var uploadedImage = await articleImageRepository.Upload(imageDomainModel);
                articleImage = uploadedImage;
            }
            else
            {
                return BadRequest("Image file is required");
            }

            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime date = DateTime.ParseExact(articleCreateRequestDto.Date, format, CultureInfo.InvariantCulture);

            var article = new Article()
            {
                Title = articleCreateRequestDto.Title,
                Date = date,
                Content = articleCreateRequestDto.Content,
                ArticleImageId = articleImage.Id,
                ImageDesc = articleCreateRequestDto.ImageDesc,
                Tags = articleCreateRequestDto.Tags,
                CategoryId = category.Id,
                Category = category,
                ArticleImage = articleImage,
            };

            await dbContext.Articles.AddAsync(article);
            await dbContext.SaveChangesAsync();

            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Date = article.Date,
                Content = article.Content,
                ArticleImageId = article.ArticleImageId,
                ImageDesc = article.ImageDesc,
                Tags = article.Tags,
                CategoryId = article.CategoryId,
            };

            //return CreatedAtAction(nameof(GetArticleById), new { id = article.Id }, articleDto);
            return Ok(articleDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var article = await dbContext.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            var articleImage = await dbContext.ArticleImages.FindAsync(article.ArticleImageId);

            if (articleImage == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), articleImage.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            dbContext.Articles.Remove(article);
            dbContext.ArticleImages.Remove(articleImage);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
