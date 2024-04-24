using MvcBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBlog.Controllers
{
    public class PostBlogController : Controller
    {
		private MvcBlogEntities db = new MvcBlogEntities();

		// GET: PostBlog
		public ActionResult Index()
        {
			var query = from p in db.Posts
						join b in db.Blogs on p.BlogId equals b.BlogId
						select new PostBlog
						{
							PostId = p.PostId,
							Title = p.Title,
							Content = p.Content,
							BlogId = b.BlogId,
							Name = b.Name
						};
			return View(query.ToList());
		}

	}
}