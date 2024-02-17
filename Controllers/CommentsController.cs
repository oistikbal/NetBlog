using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Areas.Identity.Data;
using NetBlog.Models.Data;

namespace NetBlog.Controllers
{
    public class CommentsController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly ILogger<CommentsController> _logger;
        private readonly UserManager<User> _userManager;

        public CommentsController(BlogContext blogContext, ILogger<CommentsController> logger, UserManager<User> userManager)
        {
            _blogContext = blogContext;
            _logger = logger;
            _userManager = userManager;
        }

    }
}
