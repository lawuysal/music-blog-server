using music_blog_server.Models.Domain;

namespace music_blog_server.Repositories
{
    public interface IArticleImageRepository
    {
        public Task<ArticleImage> Upload(ArticleImage image)
        {
            throw new NotImplementedException();
        }
    }
}
