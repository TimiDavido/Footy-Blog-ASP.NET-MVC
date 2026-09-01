using System.Text.Json;
using Dapper;
using Microsoft.Data.SqlClient;

namespace FootyBlog.Models
{
    public class BlogRepository
    {
        private readonly string? _connectionString;

        public BlogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public List<Blog> GetAllPosts()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Blogs";
                return connection.Query<Blog>(sql).ToList();
            }
        }

        public List<Blog> GetPosts(int page, int pageSize)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = """ SELECT * FROM Blogs ORDER BY Id DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY """;
                int offset = (page - 1) * pageSize;
                return connection.Query<Blog>(
                    sql,
                    new
                    {
                        Offset = offset,
                        PageSize = pageSize
                    } ).ToList();
            }
        }

        public int GetTotalPostsCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Blogs";
                return connection.ExecuteScalar<int>(sql);
            }
        }
        public Blog? GetPostById (int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Blogs WHERE Id = @Id";
                return connection.QueryFirstOrDefault<Blog>(
                    sql,
                    new {Id = id }
                    );
            }
        }

        public List<Blog> GetRandomPosts(int id, int count)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = """ SELECT TOP (@Count) * FROM Blogs WHERE Id != @Id ORDER BY NEWID()""";

                return connection.Query<Blog>(
                    sql,
                    new
                    {
                        Id = id,
                        Count = count
                    }).ToList();
            }
        }

        public void AddPost (Blog blog)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Blogs (Title, Content, PostDate, ImagePath) VALUES (@Title, @Content, @PostDate, @ImagePath)";
                connection.Execute(sql, blog);
            }
        }

        public void DeletePost(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Blogs WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }

        public void UpdatePost(Blog blog)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Blogs SET Title = @Title, Content = @Content, PostDate = @PostDate, ImagePath = @ImagePath WHERE Id = @Id";
                connection.Execute(sql, blog);
            }
        }
    }
}
