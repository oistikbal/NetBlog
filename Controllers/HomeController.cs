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


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
