
namespace FootyBlog.Application.Interfaces
{
        public interface ILikeRepository
        {
            Task<bool> HasLiked(string userId, int blogId);

            Task AddLike(string userId, int blogId);

            Task RemoveLike(string userId, int blogId);

            Task<int> GetLikeCount(int blogId);
    }
}
