using FootyBlog.Application.Interfaces;


namespace FootyBlog.Application.Services
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _likeRepository;

        public LikeService(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository;
        }

        public async Task<bool> ToggleLikeAsync(string userId, int blogId)
        {
            bool hasLiked = await _likeRepository.HasLikedAsync(userId, blogId);

            if (hasLiked)
            {
                await _likeRepository.RemoveLikeAsync(userId, blogId);

                return false;
            }

            await _likeRepository.AddLikeAsync(userId, blogId);

            return true;
        }

        public async Task<int> GetLikeCountAsync(int blogId)
        {
            return await _likeRepository.GetLikeCountAsync(blogId);
        }

        public async Task<bool> HasLikedAsync(string userId, int blogId)
        {
            return await _likeRepository.HasLikedAsync(userId, blogId);
        }
    }
}
