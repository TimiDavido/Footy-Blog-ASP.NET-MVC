
using FootyBlog.Domain.Entities;

namespace FootyBlog.Application.Interfaces
{
    public interface ICommentService
    {
        Task AddComment(Comment comment);
        Task<List<Comment>> GetCommentsByBlogId(int blogId);
        Task DeleteCommentById(int commentId);
    }
}
