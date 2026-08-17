using System.Security.Cryptography;
using System.Text.Json;

namespace FootyBlog.Models
{
    public class BlogRepository
    {
        private readonly string? _filePath;

        public BlogRepository(IConfiguration configuration)
        {
            _filePath = configuration["BlogSettings:FilePath"];
        }
        public List<Blog> GetAllPosts()
        {
            List<Blog>? blog = JsonSerializer.Deserialize<List<Blog>>(File.ReadAllText(_filePath));
            return blog ?? new List<Blog>();
        }

        public Blog? GetPostById (int Id)
        {
            List<Blog> blogs = GetAllPosts();
            foreach(var blog in blogs)
            {
                if (blog.Id == Id)
                {
                    return blog;
                }
            }
            return null;
        }

        public void AddPost (Blog blog)
        {
            List<Blog> blogs = GetAllPosts();

            blog.Id = blogs.Count + 1;
            blog.PostDate = DateTime.Now;

            blogs.Add(blog);
            string json = JsonSerializer.Serialize(blogs);

            File.WriteAllText(_filePath, json);
        }

        public void DeletePost(int Id)
        {
            List<Blog> posts = GetAllPosts();

            for (int i =0; i < posts.Count; i++)
            {
                if (posts[i].Id == Id)
                {
                    posts.RemoveAt(i);
                    break;
                }
            }

            string json = JsonSerializer.Serialize(posts);
            File.WriteAllText(_filePath, json);
        }

        public void UpdatePost(Blog blog)
        {
            List<Blog> posts = GetAllPosts();
            
            foreach(Blog post in posts)
            {
                if (post.Id == blog.Id)
                {
                    post.PostDate = DateTime.Now;
                    post.Content = blog.Content;
                    post.ImagePath = blog.ImagePath;
                    post.Title = blog.Title;

                    break;
                }
            }

            string json = JsonSerializer.Serialize(posts);
            File.WriteAllText(_filePath, json);
        }
    }
}
