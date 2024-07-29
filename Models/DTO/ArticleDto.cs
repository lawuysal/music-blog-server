using music_blog_server.Models.Domain;

namespace music_blog_server.Models.Dto
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public required string Content { get; set; }
        public required string ImageDesc { get; set; }
        public required List<string> Tags { get; set; }

        // Foreign key to category
        public required Guid CategoryId { get; set; }
        public required Guid ArticleImageId { get; set; }
    }
}
