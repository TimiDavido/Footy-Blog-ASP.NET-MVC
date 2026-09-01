using FootyBlog.Models;

namespace FootyBlog.ServiceContracts
{
    public interface IBlogService
    {
        //List<Blog> GetAllPosts();
        int GetTotalPostsCount();
        List<Blog> GetPosts(int page, int pageSize);
        List<Blog> GetRandomPosts(int id, int count);
        Blog? GetPostById(int id);
        void AddPost(BlogDto dto);
        void DeletePost(int id); 
        BlogDetailsViewModel GetBlogDetails(int id);
        void UpdatePost(int Id, Blog blog, IFormFile image);
    }
}
