using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using music_blog_server.Data;
using music_blog_server.Models.Domain;
using music_blog_server.Models.Dto;
using music_blog_server.Models.DTO;

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
        public IActionResult GetAllArticles()
        {
            var articles = dbContext.Articles.ToList();

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
                    Category = article.Category
                });
            }

            return Ok(articlesDto);
        }

        // Get article by id
        // GET: api/articles/id
        [HttpGet("id:guid")]
        public IActionResult GetArticleById([FromRoute] Guid id)
        {
            var article = dbContext.Articles.Find(id);

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
                Category = article.Category
            };

            return Ok(articleDto);
        }

        // Create article
        // POST: api/articles
        [HttpPost]
        public IActionResult CreateArticle([FromBody] ArticleCreateRequestDto articleCreateRequestDto)
        {
            var article = new Article()
            {
                Title = articleCreateRequestDto.Title,
                Date = articleCreateRequestDto.Date,
                Content = articleCreateRequestDto.Content,
                ImageUrl = articleCreateRequestDto.ImageUrl,
                ImageDesc = articleCreateRequestDto.ImageDesc,
                Tags = articleCreateRequestDto.Tags,
                Category = articleCreateRequestDto.Category
            };

            dbContext.Articles.Add(article);
            dbContext.SaveChanges();

            var articleDto = new ArticleDto()
            {
                Id = article.Id,
                Title = article.Title,
                Date = article.Date,
                Content = article.Content,
                ImageUrl = article.ImageUrl,
                ImageDesc = article.ImageDesc,
                Tags = article.Tags,
                Category = article.Category
            };

            return CreatedAtAction(nameof(GetArticleById), new { id = article.Id }, articleDto);
        }
    }
}
