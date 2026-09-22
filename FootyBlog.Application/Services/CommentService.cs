using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;

namespace FootyBlog.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task AddComment(Comment comment)
        {
            await _commentRepository.AddComment(comment);
        }
        public async Task<List<Comment>> GetCommentsByBlogId(int blogId)
        {
            return await _commentRepository.GetCommentsByBlogId(blogId);
        }
        public async Task DeleteCommentById(int commentId)
        {
            await _commentRepository.DeleteCommentById(commentId);
        }
    }
}
