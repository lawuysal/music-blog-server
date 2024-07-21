using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_blog_server.Data;
using music_blog_server.Models.Domain;
using music_blog_server.Models.DTO;

namespace music_blog_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MusicBlogDbContext dbContext;

        public CategoriesController(MusicBlogDbContext dbContext) 
        { 
            this.dbContext = dbContext;
        }

        // Get all categories
        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAllCategories() 
        {
            var categories = await dbContext.Categories.ToListAsync();

            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto() 
                { 
                    Id = category.Id,
                    Name = category.Name,                
                });
            }

            return Ok(categoriesDto);
        }

        // Get category by id
        // GET: api/categories/id
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id) 
        {
            var category = await dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(categoryDto);
        }

        // Create category
        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateRequestDto categoryCreateRequestDto)
        {
            var category = new Category()
            {
                Name = categoryCreateRequestDto.Name,
            };

            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            var categoryDto = new CategoryDto() 
            { 
                Id = category.Id, 
                Name = category.Name, 
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, categoryDto);
        }
    }
}
