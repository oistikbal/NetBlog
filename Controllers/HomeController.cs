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

    public async Task<IActionResult> Index()
    {
        var posts = await _blogContext.Posts
            .OrderByDescending(p => p.Id)
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

        return View(postViews);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
