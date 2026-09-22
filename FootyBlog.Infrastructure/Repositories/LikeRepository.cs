using Dapper;
using FootyBlog.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FootyBlog.Infrastructure.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly string? _connectionString;

    public LikeRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<bool> HasLikedAsync(string userId, int blogId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = @"
            SELECT COUNT(*)
            FROM Likes
            WHERE UserId = @UserId
            AND BlogId = @BlogId";

        int count = await connection.ExecuteScalarAsync<int>(
            sql,
            new
            {
                UserId = userId,
                BlogId = blogId
            });

        return count > 0;
    }

    public async Task AddLikeAsync(string userId, int blogId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = @"
            INSERT INTO Likes (UserId, BlogId)
            VALUES (@UserId, @BlogId)";

        await connection.ExecuteAsync(
            sql,
            new
            {
                UserId = userId,
                BlogId = blogId
            });
    }

    public async Task RemoveLikeAsync(string userId, int blogId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = @"
            DELETE FROM Likes
            WHERE UserId = @UserId                                                                                                                                                                                                                                                                                       
            AND BlogId = @BlogId";

        await connection.ExecuteAsync(
            sql,
            new
            {
                UserId = userId,
                BlogId = blogId
            });
    }

    public async Task<int> GetLikeCountAsync(int blogId)
    {
        using SqlConnection connection = new SqlConnection(_connectionString);

        string sql = @"
            SELECT COUNT(*)
            FROM Likes
            WHERE BlogId = @BlogId";

        return await connection.ExecuteScalarAsync<int>(
            sql,
            new { BlogId = blogId });
    }
}