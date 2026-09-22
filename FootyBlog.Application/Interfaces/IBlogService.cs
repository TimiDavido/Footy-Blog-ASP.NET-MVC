using FootyBlog.Application.DTOs;
using FootyBlog.Application.ViewModels;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FootyBlog.Application.Interfaces
{
    public interface IBlogService
    {
        Task<int> GetTotalPostsCount();
        Task<List<Blog>> GetPosts(int page, int pageSize);
        Task<Blog?> GetPostById(int id);
        Task AddPost(BlogDto dto);
        Task DeletePost(int id);
        Task<BlogDetailsViewModel> GetBlogDetails(int id, string userId);
        Task UpdatePost(int Id, Blog blog, IFormFile image);
    }
}

