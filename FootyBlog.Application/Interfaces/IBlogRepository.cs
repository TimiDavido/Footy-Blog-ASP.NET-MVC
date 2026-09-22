using FootyBlog.Domain.Entities;

namespace FootyBlog.Application.Interfaces
{
    public interface IBlogRepository
    {
        Task<List<Blog>> GetPosts(int page, int pageSize);
        Task<int> GetTotalPostsCount();
        Task<List<Blog>> GetRandomPosts(int id, int count);
        Task<Blog?> GetPostById(int id);
        Task AddPost(Blog blog);
        Task DeletePost(int id);
        Task UpdatePost(Blog blog);
    }
}
