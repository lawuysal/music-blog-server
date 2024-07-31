using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using music_blog_server.Data;
using music_blog_server.Models.Domain;
using music_blog_server.Models.DTO;
using music_blog_server.Repositories;

namespace music_blog_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleryImagesController : ControllerBase
    {
        private readonly IGalleryImageRepository imageRepsitory;
        private readonly MusicBlogDbContext dbContext;

        public GalleryImagesController(IGalleryImageRepository imageRepsitory, MusicBlogDbContext dbContext)
        {
            this.imageRepsitory = imageRepsitory;
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllGalleryImages()
        {
            var galleryImages = await dbContext.GalleryImages.ToListAsync();
            galleryImages = galleryImages.OrderByDescending(x => x.FileCreatedAt).ToList();

            var galleryImagesDto = new List<GalleryImageDto>();

            foreach (var image in galleryImages) {

                galleryImagesDto.Add(new GalleryImageDto
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    FileExtension = image.FileExtension,
                    FilePath = image.FilePath,
                    FileSizeInBytes = image.FileSizeInBytes,
                    FileCreatedAt = image.FileCreatedAt,
                });
            }

            return Ok(galleryImagesDto);

        }

        [Authorize]
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] GalleryImageUploadRequestDto request)
        {
            var imageDomainModel = new GalleryImage
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName.ToLower().Trim().Replace(" ", "-"),
                FileCreatedAt = DateTime.Now,
            };

            await imageRepsitory.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGalleryImage([FromRoute] Guid id)
        {
            var galleryImage = await dbContext.GalleryImages.FindAsync(id);

            if (galleryImage == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), galleryImage.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            dbContext.GalleryImages.Remove(galleryImage);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
