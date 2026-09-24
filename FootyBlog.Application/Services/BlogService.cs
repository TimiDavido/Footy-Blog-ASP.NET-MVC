using FootyBlog.Application.DTOs;
using FootyBlog.Application.Interfaces;
using FootyBlog.Application.ViewModels;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FootyBlog.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ILikeService _likeService;
        private readonly ICommentRepository _commentRepository;

        public BlogService(IBlogRepository blogRepository, ILikeService likeService, ICommentRepository commentRepository)
        {
            _blogRepository = blogRepository;
            _likeService = likeService;
            _commentRepository = commentRepository;
        }

        public async Task<List<Blog>> GetPosts(int page, int pageSize)
        {
            return await _blogRepository.GetPosts(page, pageSize);
        }

        public async Task<int> GetTotalPostsCount()
        {
            return await _blogRepository.GetTotalPostsCount();
        }

        public async Task<Blog?> GetPostById(int id)
        {
            return await _blogRepository.GetPostById(id);
        }

        public async Task AddPost(BlogDto dto)
        {
            if (dto.Image == null)
            {
                throw new Exception("Image is required.");
            }

            string folder = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images");

            string filePath = Path.Combine(folder, dto.Image.FileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }

            Blog blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                ImagePath = "/images/" + dto.Image.FileName,
                PostDate = DateTime.Now
            };

            await _blogRepository.AddPost(blog);
        }
        public async Task DeletePost(int id)
        {
            await _blogRepository.DeletePost(id);
        }
        public async Task<BlogDetailsViewModel> GetBlogDetails(int id, string? userId)
        {
            Blog? post = await _blogRepository.GetPostById(id);

            List<Blog> otherPosts = await _blogRepository.GetRandomPosts(id, 8);
            List<Comment> comments = await _commentRepository.GetCommentsByBlogId(id);

            int likeCount = await _likeService.GetLikeCount(id);

            bool hasLiked = false;

            if (userId != null)
            {
                hasLiked = await _likeService.HasLiked(userId, id);
            }

            return new BlogDetailsViewModel
            {
                Blog = post,
                OtherPosts = otherPosts,
                LikeCount = likeCount,
                HasLiked = hasLiked,
                Comments = comments
            };
        }
        public async Task UpdatePost(int Id, Blog blog, IFormFile image)
        {
            Blog? existingBlog = await _blogRepository.GetPostById(Id);
            if (existingBlog == null)
            {
                throw new Exception("Blog post not found");
            }

            existingBlog.Title = blog.Title;
            existingBlog.Content = blog.Content;

            if (image != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string filePath = Path.Combine(folder, image.FileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                existingBlog.ImagePath = "/images/" + image.FileName;
            }
            await _blogRepository.UpdatePost(existingBlog);
        }
    }
}


