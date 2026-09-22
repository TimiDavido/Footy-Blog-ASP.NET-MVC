
using Dapper;
using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FootyBlog.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly string? _connectionString;

        public CommentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task AddComment(Comment comment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO Comments (BlogId, UserId, Content, CreatedAt)
                    VALUES (@BlogId, @UserId, @Content, @CreatedAt)";
                    
                await connection.ExecuteAsync(
                    sql,
                    new
                    {
                        comment.userId,
                        comment.blogId,
                        comment.Content,
                        CreatedAt = DateTime.UtcNow
                    });
            }

        }

        public async Task<List<Comment>> GetCommentsByBlogId(int blogId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT Id, BlogId, UserId, Content, CreatedAt
                    FROM Comments
                    WHERE BlogId = @BlogId
                    ORDER BY CreatedAt DESC";

                var comments = (await connection.QueryAsync<Comment>(
                    sql,
                    new { BlogId = blogId })).ToList();

                return comments;
            }
        }

        public async Task DeleteCommentById(int commentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    DELETE FROM Comments
                    WHERE Id = @CommentId";
                await connection.ExecuteAsync(
                    sql,
                    new { CommentId = commentId });
            }
        }
    }
}
