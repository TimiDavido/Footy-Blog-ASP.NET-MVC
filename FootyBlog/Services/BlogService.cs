using FootyBlog.Models;
using FootyBlog.ServiceContracts;

namespace FootyBlog.Services
{
    public class BlogService : IBlogService
    {
        private readonly BlogRepository _blogRepository;
        public BlogService(BlogRepository blogRepository)
        {
             _blogRepository = blogRepository;
        }

        //public List<Blog> GetAllPosts()
        //{
        //    return _blogRepository.GetAllPosts();
        //}

        public List<Blog> GetPosts(int page, int pageSize)
        {
            return _blogRepository.GetPosts(page, pageSize);
        }

        public List<Blog> GetRandomPosts(int id, int count)
        {
            return _blogRepository.GetRandomPosts(id, count);
        }
        public int GetTotalPostsCount()
        {
            return _blogRepository.GetTotalPostsCount();
        }

        public Blog? GetPostById(int id)
        {
            return _blogRepository.GetPostById(id);
        }

        public void AddPost(BlogDto dto)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images");

            string filePath = Path.Combine(folder, dto.Image.FileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }

            Blog blog = new Blog
            {
                Title = dto.Title,
                Content = dto.Content,
                ImagePath = "/images/" + dto.Image.FileName,
                PostDate = DateTime.Now
            };

            _blogRepository.AddPost(blog);
        }
        public void DeletePost(int id)
        {
            _blogRepository.DeletePost(id);
        }

        public BlogDetailsViewModel GetBlogDetails(int id)
        {
            Blog? post = _blogRepository.GetPostById(id);
            List<Blog> otherPosts = _blogRepository.GetRandomPosts(id, 6);

            return new BlogDetailsViewModel
            {
                Blog = post,
                OtherPosts = otherPosts
            };
        }
        public void UpdatePost(int Id, Blog blog, IFormFile image)
        {
            Blog? existingBlog = _blogRepository.GetPostById(Id);
            if (existingBlog == null)
            {
                throw new Exception("Blog post not found");
            }

            existingBlog.Title = blog.Title;
            existingBlog.Content = blog.Content;

            if (image != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                string filePath = Path.Combine(folder, image.FileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                existingBlog.ImagePath = "/images/" + image.FileName;
            }
            _blogRepository.UpdatePost(existingBlog);
        }
    }
}
