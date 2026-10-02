using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace FootyBlog.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LikeService(ILikeRepository likeRepository, IBlogRepository blogRepository, IEmailService emailService, UserManager<ApplicationUser> userManager)
        {
            _likeRepository = likeRepository;
            _blogRepository = blogRepository;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<bool> ToggleLike(string userId, int blogId)
        {
            bool hasLiked = await _likeRepository.HasLiked(userId, blogId);

            if (hasLiked)
            {
                await _likeRepository.RemoveLike(userId, blogId);

                return false;
            }

            await _likeRepository.AddLike(userId, blogId);

            var blog = await _blogRepository.GetPostById(blogId);

            if (blog != null)
            {
                var author = await _userManager.FindByIdAsync(blog.UserId!);
                if (author != null)
                {
                    var emailRequest = new SendSingleMailRequest
                    {
                        Receiver = author.Email,
                        Subject = "Your post received a new like!",
                        Body = $"Hello {author.UserName},\n\nYour post titled '{blog.Title}' has received a new like!"
                    };
                    await _emailService.SendEmail(emailRequest);
                }
            }
            return true;
        }

        public async Task<int> GetLikeCount(int blogId)
        {
            return await _likeRepository.GetLikeCount(blogId);
        }

        public async Task<bool> HasLiked(string userId, int blogId)
        {
            return await _likeRepository.HasLiked(userId, blogId);
        }
    }
}
