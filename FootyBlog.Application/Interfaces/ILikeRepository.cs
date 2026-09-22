
namespace FootyBlog.Application.Interfaces
{
        public interface ILikeRepository
        {
            Task<bool> HasLikedAsync(string userId, int blogId);

            Task AddLikeAsync(string userId, int blogId);

            Task RemoveLikeAsync(string userId, int blogId);

            Task<int> GetLikeCountAsync(int blogId);
    }
}
