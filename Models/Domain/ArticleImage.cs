using System.ComponentModel.DataAnnotations.Schema;

namespace music_blog_server.Models.Domain
{
    public class ArticleImage
    {
        public Guid Id { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
        public DateTime FileCreatedAt { get; set; }
    }
}

