using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Areas.Identity.Data;
using NetBlog.Models;
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


		[Route("[controller]/Create")]
		[HttpPost]
		public async Task<ActionResult> Create([Bind("PostId,Body")] CommentInput commentInput)
		{

			if (!ModelState.IsValid)
				return RedirectToAction("Show", "Posts", new { id = commentInput.PostId});

			var comment = new Comment();
			comment.User = await _userManager.GetUserAsync(this.User);
			comment.Body = commentInput.Body;
			comment.PostId = commentInput.PostId;

			try
			{
				_blogContext.Comments.Add(comment);
				await _blogContext.SaveChangesAsync();
				return RedirectToAction("Show", "Posts", new { id = commentInput.PostId });
			}
			catch (Exception e)
			{
				_logger.LogError(e, e.Message);
			}

			return RedirectToAction("Index", "Posts");
		}
	}
}
