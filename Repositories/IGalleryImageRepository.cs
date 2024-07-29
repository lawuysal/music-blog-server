using music_blog_server.Models.Domain;

namespace music_blog_server.Repositories
{
    public interface IGalleryImageRepository
    {
        public Task<GalleryImage> Upload(GalleryImage image)
        {
            throw new NotImplementedException();
        }
    }
}
