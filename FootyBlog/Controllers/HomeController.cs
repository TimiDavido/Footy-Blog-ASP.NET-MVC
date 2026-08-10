using FootyBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootyBlog.Controllers
{
    public class HomeController : Controller
    {
        private BlogRepository blogRepository = new BlogRepository();

        [Route("/index")]
        [Route("/")]
        public IActionResult Index()
        {
            List<Blog> posts = blogRepository.GetAllPosts();
            return View(posts);
        }

        [Route("/details/{Id}")]
        public IActionResult Details(int Id)
        {
            Blog? post = blogRepository.GetPostById(Id);
            List<Blog> otherPosts = blogRepository.GetAllPosts();
            List<Blog> allPosts = new List<Blog>();

            foreach (Blog blog in otherPosts)
            {
                if (blog.Id != Id)
                {
                    allPosts.Add(blog);
                }
            }

            BlogDetailsViewModel model = new BlogDetailsViewModel
            {
                Blog = post,
                OtherPosts = allPosts
            };
            return View(model);
        }

        [Route("/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/create")]
        public IActionResult Create(Blog blog, IFormFile image)
        {
            if(!ModelState.IsValid || image == null)
            {
                return View();
            }

            string folder = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "images");

                string filePath = Path.Combine(folder, image.FileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                blog.ImagePath = "/images/" + image.FileName;

            blogRepository.AddPost(blog);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/delete/{Id}")]
        public IActionResult Delete(int Id)
        {
            blogRepository.DeletePost(Id);
            return RedirectToAction("index");
        }
    }
}
