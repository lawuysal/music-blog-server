using music_blog_server.Models.Domain;

namespace music_blog_server.Models.DTO
{
    public class ArticleCreateRequestDto
    {
        public required string Title { get; set; }
        public required string Date { get; set; }
        public required string Content { get; set; }
        public required string ImageUrl { get; set; }
        public required string ImageDesc { get; set; }
        public required List<string> Tags { get; set; }

        // Foreign key to category
        public required Guid CategoryId { get; set; }
    }
}
