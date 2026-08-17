using FootyBlog.Models;
using FootyBlog.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace FootyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogService _blogService;

        public HomeController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [Route("/index")]
        [Route("/")]
        public IActionResult Index()
        {
            List<Blog> posts = _blogService.GetAllPosts();
            return View(posts);
        }

        [Route("/details/{Id}")]
        public IActionResult Details(int Id)
        {
            BlogDetailsViewModel model = _blogService.GetBlogDetails(Id);
            return View(model);
        }

        [Route("/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/create")]
        public IActionResult Create(BlogDto dto)
        {
            if(!ModelState.IsValid)
            {
                return View(dto);
            }

            _blogService.AddPost(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/delete/{Id}")]
        public IActionResult Delete(int Id)
        {
            _blogService.DeletePost(Id);
            return RedirectToAction("index");
        }

        [Route("/edit/{Id}")]
        public IActionResult Edit(int Id)
        {
            Blog? blog = _blogService.GetPostById(Id);
            if(blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [Route("/edit/{Id}")]
        public IActionResult Edit(int Id, Blog blog, IFormFile image)
        {
            _blogService.UpdatePost(Id, blog, image);
            return RedirectToAction("Details", new { Id = Id });
        }
    }
}
