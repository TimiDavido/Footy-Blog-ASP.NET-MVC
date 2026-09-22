
namespace FootyBlog.Application.Interfaces
{
    public interface ILikeService
    {
        Task<bool> ToggleLikeAsync(string userId, int blogId);

        Task<int> GetLikeCountAsync(int blogId);

        Task<bool> HasLikedAsync(string userId, int blogId);
    }
}
