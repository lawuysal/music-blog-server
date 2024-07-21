namespace music_blog_server.Models.Domain
{
    public class Article
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public required string Content { get; set; }
        public required string ImageUrl { get; set; }
        public required string ImageDesc { get; set; }  
        public required List<string> Tags { get; set; }
        public required Category Category { get; set; }
    }
}
