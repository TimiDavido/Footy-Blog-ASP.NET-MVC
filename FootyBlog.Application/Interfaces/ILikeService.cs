
namespace FootyBlog.Application.Interfaces
{
    public interface ILikeService
    {
        Task<bool> ToggleLike(string userId, int blogId);

        Task<int> GetLikeCount(int blogId);

        Task<bool> HasLiked(string userId, int blogId);
    }
}
