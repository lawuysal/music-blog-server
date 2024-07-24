using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_blog_server.Data;
using music_blog_server.Models.Domain;
using music_blog_server.Models.Dto;
using music_blog_server.Models.DTO;
using System.Globalization;

namespace music_blog_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly MusicBlogDbContext dbContext;

        public ArticlesController(MusicBlogDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Get all articles
        // GET: api/articles
        [HttpGet]
        public async Task<IActionResult> GetAllArticles()
        {
            var articles = await dbContext.Articles.ToListAsync();
            articles = articles.OrderByDescending(x => x.Date).ToList();

            var articlesDto = new List<ArticleDto>();
            foreach(var article in articles)
            {
                articlesDto.Add(new ArticleDto()
                {
                    Id = article.Id,
                    Title = article.Title,
                    Date = article.Date,
                    Content = article.Content,
                    ImageUrl = article.ImageUrl,
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

            if(article == null)
            {
                return NotFound();
            }

            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Date = article.Date,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                ImageDesc = article.ImageDesc,
                Tags = article.Tags,
                CategoryId = article.CategoryId,
            };

            return Ok(articleDto);
        }

        // Create article
        // POST: api/articles
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleCreateRequestDto articleCreateRequestDto)
        {
            var category = await dbContext.Categories.FindAsync(articleCreateRequestDto.CategoryId);

            if (category == null)
            {
                return BadRequest("Invalid CategoryId: Category not found");
            }

            string format = "yyyy-MM-dd HH:mm:ss";
            DateTime date = DateTime.ParseExact(articleCreateRequestDto.Date, format, CultureInfo.InvariantCulture);

            var article = new Article()
            {
                Title = articleCreateRequestDto.Title,
                Date = date,
                Content = articleCreateRequestDto.Content,
                ImageUrl = articleCreateRequestDto.ImageUrl,
                ImageDesc = articleCreateRequestDto.ImageDesc,
                Tags = articleCreateRequestDto.Tags,
                CategoryId = category.Id,
                Category = category
            };

            await dbContext.Articles.AddAsync(article);
            await dbContext.SaveChangesAsync();

            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Date = article.Date,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                ImageDesc = article.ImageDesc,
                Tags = article.Tags,
                CategoryId = article.CategoryId,
            };

            return CreatedAtAction(nameof(GetArticleById), new { id = article.Id }, articleDto);
        }
    }
}
