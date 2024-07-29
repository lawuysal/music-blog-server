namespace music_blog_server.Models.DTO
{
    public class ArticleImageDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
        public DateTime FileCreatedAt { get; set; }
    }
}
