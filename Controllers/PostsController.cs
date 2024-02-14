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
                return RedirectToAction("Index","Home");

            var user = await _userManager.GetUserAsync(this.User);

			if (user != null && post.User?.Id == user.Id)
            {
                var postInput = new PostInput();
                postInput.Title = post.Title;
                postInput.Body = post.Body;
                return View(postInput);
            }

			return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");

            var user = await _userManager.GetUserAsync(this.User);

            if (user != null && post.User?.Id == user.Id)
            {
                post.Title = postInput.Title;
                post.Body = postInput.Body;
                await _blogContext.SaveChangesAsync();

                return RedirectToAction(nameof(Show), new { id = post.Id });
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("[controller]/{id?}")]
        [Authorize]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
