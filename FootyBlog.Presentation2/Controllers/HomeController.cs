using FootyBlog.Application.DTOs;
using FootyBlog.Application.Interfaces;
using FootyBlog.Application.Services;
using FootyBlog.Application.ViewModels;
using FootyBlog.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FootyBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ILikeService _likeService;
        private readonly ICommentService _commentService;

        public HomeController(IBlogService blogService, ILikeService likeService, ICommentService commentService)
        {
            _blogService = blogService;
            _likeService = likeService;
            _commentService = commentService;
        }

        [Route("/index")]
        [Route("/")]
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 9;
            List<Blog> posts = await _blogService.GetPosts(page, pageSize);

            int totalPosts = await _blogService.GetTotalPostsCount();

            int totalPages = (int)Math.Ceiling((double)totalPosts / pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            return View(posts);
        }

        [Route("/details/{Id}")]
        public async Task<IActionResult> Details(int Id)
        {
            string? userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier
            )?.Value;

            BlogDetailsViewModel model =
                await _blogService.GetBlogDetails(Id, userId);

            return View(model);
        }

        [Authorize(Roles ="Admin")]
        [Route("/create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> Create(BlogDto dto)
        {
            if(!ModelState.IsValid)
            {
                return View(dto);
            }

            await _blogService.AddPost(dto);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/delete/{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _blogService.DeletePost(Id);
            return RedirectToAction("index");
        }

        [Authorize(Roles = "Admin")]
        [Route("/edit/{Id}")]
        public async Task<IActionResult> Edit(int Id)
        {
            Blog? blog = await _blogService.GetPostById(Id);
            if(blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("/edit/{Id}")]
        public async Task<IActionResult> Edit(int Id, Blog blog, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View(blog);
            }
            await _blogService.UpdatePost(Id, blog, image);
            return RedirectToAction("Details", new { Id = Id });
        }

        [Authorize]
        [HttpPost]
        [Route("/like/{Id}")]
        public async Task<IActionResult> Like(int Id)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            await _likeService.ToggleLikeAsync(userId, Id);

            return RedirectToAction("Details", new { Id = Id });
        }

        [Authorize]
        [HttpPost]
        [Route("/comment/addcomment")]
        public async Task<IActionResult> AddComment (int blogId, string content)
        {
            string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;

            var comment = new Comment
            {
                Content = content,
                blogId = blogId,
                userId = userId
            };

            await _commentService.AddComment(comment);

            return RedirectToAction("Details", new { Id = blogId });
        }
    }
}
