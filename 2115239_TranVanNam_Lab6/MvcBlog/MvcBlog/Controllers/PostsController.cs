using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;

namespace MvcBlog.Controllers
{
    public class PostsController : Controller
    {
        private MvcBlogEntities db = new MvcBlogEntities();

		// GET: Posts
		public ActionResult Index(string title, int? blogId, DateTime? fromDate, DateTime? toDate)
		{
			var posts = db.Posts.AsQueryable();
			DateTime now = DateTime.Now;

			if (blogId.HasValue)
			{
				posts = posts.Where(s => s.BlogId == blogId.Value);
			}
			if (!String.IsNullOrEmpty(title))
			{
				posts = posts.Where(s => s.Title.Contains(title));
			}
			
			if (fromDate.HasValue && fromDate > now)
			{
				ModelState.AddModelError("fromDate", "Ngày 'từ' không thể ở trong tương lai.");
			}
			
			if (toDate.HasValue && fromDate.HasValue && toDate < fromDate)
			{
				ModelState.AddModelError("toDate", "Ngày 'đến' phải lớn hơn hoặc bằng ngày 'từ'.");
			}
			
			if (ModelState.IsValid)
			{
				if (fromDate.HasValue)
				{
					posts = posts.Where(s => s.CreateDate >= fromDate.Value);
				}
				if (toDate.HasValue)
				{
					posts = posts.Where(s => s.CreateDate <= toDate.Value);
				}
			}

			ViewData["BlogId"] = new SelectList(db.Blogs, "BlogId", "Name");

			return View(posts.ToList());
		}











		// GET: Posts/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Name");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,Title,Content,BlogId,CreateDate")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Name", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Name", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Content,BlogId,CreateDate")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogId = new SelectList(db.Blogs, "BlogId", "Name", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
