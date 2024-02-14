using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetBlog.Areas.Identity.Data;
using NetBlog.Models;
using NetBlog.Models.Data;

namespace NetBlog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BlogContext _blogContext;
    private readonly UserManager<User> _userManager;

    public HomeController(BlogContext blogContext, ILogger<HomeController> logger, UserManager<User> userManager)
    {
        _blogContext = blogContext;
        _logger = logger;
        _userManager = userManager;
    }

    [Route("/")]
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]int page = 1)
    {
        const int pageSize = 10;

        var posts = await _blogContext.Posts
            .OrderByDescending(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(10)
            .ToListAsync();

        var postViews = new List<PostView>();
        foreach (var post in posts)
        {
            var user = await _userManager.FindByIdAsync(post.UserId);
            var postView = new PostView
            {
                Title = post.Title,
                Body = post.Body,
                Email = user?.Email
            };
            postViews.Add(postView);
        }

        var totalPosts = _blogContext.Posts.Count();
        var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

        var model = new PostsView
        {
            Posts = postViews,
            CurrentPage = page,
            TotalPages = totalPages
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
