using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Data;
using NetBlog.Models.Data;

namespace NetBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly UserContext _userContext;

        public PostsController(BlogContext blogContext, UserContext userContext) 
        { 
            _blogContext = blogContext;
            _userContext = userContext;
        }


        [Route("[controller]/{id?}")]
        [HttpGet]
        public ActionResult Show(int id)
        {
            return View();
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
        public ActionResult Create(IFormCollection collection)
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
