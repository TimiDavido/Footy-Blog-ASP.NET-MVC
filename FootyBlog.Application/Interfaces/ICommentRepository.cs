using FootyBlog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootyBlog.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task AddComment(Comment comment);
        Task<List<Comment>> GetCommentsByBlogId(int blogId);
        Task DeleteCommentById(int commentId);
    }
}
