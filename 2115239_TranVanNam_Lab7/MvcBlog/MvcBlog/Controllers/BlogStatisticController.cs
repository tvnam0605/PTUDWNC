using MvcBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcBlog.Controllers
{
	public class BlogStatisticController : Controller
	{
		private MvcBlogEntities db = new MvcBlogEntities();

		// GET: BlogStatistic
		public ActionResult Index()
		{
			var statistics = db.Blogs
				.GroupBy(b => b.Owner)
				.Select(group => new BlogStatistic
				{
					Owner = group.Key,
					BlogCount = group.Count()
				}).ToList();

			return View(statistics);
		}
	}
}