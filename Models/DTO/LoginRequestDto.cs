using System.ComponentModel.DataAnnotations;

namespace music_blog_server.Models.DTO
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
