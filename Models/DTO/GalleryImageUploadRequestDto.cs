using System.ComponentModel.DataAnnotations;

namespace music_blog_server.Models.DTO
{
    public class GalleryImageUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public string FileName { get; set; }

    }
}
