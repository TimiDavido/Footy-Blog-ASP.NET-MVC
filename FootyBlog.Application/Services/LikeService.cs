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

        public async Task<bool> ToggleLike(string userId, int blogId)
        {
            bool hasLiked = await _likeRepository.HasLiked(userId, blogId);

            if (hasLiked)
            {
                await _likeRepository.RemoveLike(userId, blogId);

                return false;
            }

            await _likeRepository.AddLike(userId, blogId);

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
