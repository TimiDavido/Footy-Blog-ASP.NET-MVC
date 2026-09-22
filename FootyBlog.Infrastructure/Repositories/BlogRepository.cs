using Dapper;
using FootyBlog.Application.Interfaces;
using FootyBlog.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace FootyBlog.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly string? _connectionString;

        public BlogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Blog>> GetPosts(int page, int pageSize)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = " SELECT * FROM Blogs ORDER BY Id DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ";
                int offset = (page - 1) * pageSize;
                var posts = await connection.QueryAsync<Blog>(
                    sql,
                    new
                    {
                        Offset = offset,
                        PageSize = pageSize
                    });
                return posts.ToList();
            }
        }

        public async Task<int> GetTotalPostsCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Blogs";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }
        public async Task<Blog?> GetPostById (int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Blogs WHERE Id = @Id";
                var post =  await connection.QueryFirstOrDefaultAsync<Blog>(
                    sql,
                    new {Id = id }
                    );
                return post;
            }
        }

        public async Task<List<Blog>> GetRandomPosts(int id, int count)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = """ SELECT TOP (@Count) * FROM Blogs WHERE Id != @Id ORDER BY NEWID()""";

                var posts = await connection.QueryAsync<Blog>(
                    sql,
                    new
                    {
                        Id = id,
                        Count = count
                    });
                return posts.ToList();
            }
        }

        public async Task AddPost (Blog blog)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Blogs (Title, Content, PostDate, ImagePath) VALUES (@Title, @Content, @PostDate, @ImagePath)";
                await connection.ExecuteAsync(sql, blog);
            }
        }

        public async Task DeletePost(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Blogs WHERE Id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task UpdatePost(Blog blog)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Blogs SET Title = @Title, Content = @Content, PostDate = @PostDate, ImagePath = @ImagePath WHERE Id = @Id";
                await connection.ExecuteAsync(sql, blog);
            }
        }
    }
}
