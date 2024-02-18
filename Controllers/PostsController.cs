using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetBlog.Areas.Identity.Data;
using NetBlog.Models;
using NetBlog.Models.Data;

namespace NetBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly ILogger<PostsController> _logger;
        private readonly UserManager<User> _userManager;

        public PostsController(BlogContext blogContext, ILogger<PostsController> logger, UserManager<User> userManager)
        {
            _blogContext = blogContext;
            _logger = logger;
            _userManager = userManager;
        }

        [Route("/")]
        [Route("[controller]")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1)
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
                    Email = user?.Email,
                    Id = post.Id
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

        [Route("[controller]/{id?}")]
        [HttpGet]
        public async Task<ActionResult> Show(int id)
        {
            var post = await _blogContext.Posts.FirstOrDefaultAsync(post => post.Id == id);

            if(post == null)
                return Redirect("/");

			var postUser = await _userManager.FindByIdAsync(post.UserId);

			var postView = new PostView();
			postView.Title = post.Title;
            postView.Body = post.Body;
            postView.Email = postUser.Email;
            postView.Comments = await GetCommentsAsync(id);
			postView.Id = post.Id;

			return View(postView);
		}

		[Route("[controller]/New")]
		[HttpGet]
		[Authorize]
		public ActionResult New()
        {
            return View();
        }

        [Route("[controller]/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,Body")] PostInput postInput)
        {
            if (!ModelState.IsValid)
                return View(nameof(New));

            var post = new Post();
            post.User = await _userManager.GetUserAsync(this.User);
            post.Title = postInput.Title;
            post.Body = postInput.Body;

            try
            {
                _blogContext.Posts.Add(post);
                await _blogContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
            }

            return RedirectToAction(nameof(Show), new { id = post.Id }); ;
        }

		[Route("[controller]/{id?}/edit")]
		[HttpGet]
		[Authorize]
		public async Task<ActionResult> Edit(int id)
        {
            var post = await _blogContext.Posts.FindAsync(id);

            if (post == null)
                return RedirectToAction(nameof(Index));

            var user = await _userManager.GetUserAsync(this.User);

			if (user != null && post.User?.Id == user.Id)
            {
                var postInput = new PostInput();
                postInput.Title = post.Title;
                postInput.Body = post.Body;
                return View(postInput);
            }

			return RedirectToAction(nameof(Index));
		}

        [Route("[controller]/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Update(int id, [Bind("Title,Body")] PostInput postInput)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Edit), new { id = id });

            var post = await _blogContext.Posts.FindAsync(id);

            if (post == null)
                return RedirectToAction(nameof(Index));

            var user = await _userManager.GetUserAsync(this.User);

            if (user != null && post.User?.Id == user.Id)
            {
                post.Title = postInput.Title;
                post.Body = postInput.Body;
                await _blogContext.SaveChangesAsync();

                return RedirectToAction(nameof(Show), new { id = post.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        [Route("[controller]/{id?}")]
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
			var post = await _blogContext.Posts.FindAsync(id);

			if (post == null)
				return NotFound();

			var user = await _userManager.GetUserAsync(this.User);

			if (user != null && post.User?.Id == user.Id)
			{
				_blogContext.Posts.Remove(post);
				await _blogContext.SaveChangesAsync();

				return Ok();
			}
            else
            {
				return Unauthorized();
			}
		}

        [NonAction]
		private async Task<ICollection<CommentView>> GetCommentsAsync(int id)
		{
			var comments = await _blogContext.Comments
				.Where(c => c.PostId == id)
				.Select(c => new CommentView
				{
					Body = c.Body,
					Email = c.UserId != null ? _userManager.FindByIdAsync(c.UserId).Result.Email : null
				})
				.ToListAsync();

			return comments;
		}
	}
}
