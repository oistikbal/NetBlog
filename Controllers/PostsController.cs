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

			var postView = new PostViewModel();
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

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,Body")] PostInput postInput)
        {
            if (ModelState.IsValid)
            {
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
            else
            {
                return View(nameof(New), postInput);
            }
        }

		[Route("[controller]/edit/{id?}")]
		[HttpGet]
		[Authorize]
		public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id, IFormCollection collection)
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

        [HttpPost]
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
