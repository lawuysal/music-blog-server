using music_blog_server.Models.Domain;

namespace music_blog_server.Repositories
{
    public interface IGalleryImageRepsitory
    {
        public Task<GalleryImage> Upload(GalleryImage image)
        {
            throw new NotImplementedException();
        }
    }
}
