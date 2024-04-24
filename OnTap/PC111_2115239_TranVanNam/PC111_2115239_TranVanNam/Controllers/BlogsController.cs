using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PC111_2115239_TranVanNam.Models;

namespace PC111_2115239_TranVanNam.Controllers
{
    public class BlogsController : Controller
    {
        private DBOnTapEntities db = new DBOnTapEntities();

		// GET: Blogs
		public ActionResult Index(string searchString, string sortOrder, int? page, int? size)
		{
			// Sắp xếp
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.UrlSortParm = sortOrder == "Url" ? "url_desc" : "Url";

			var blogs = from b in db.Blogs
						select b;

			// Tìm kiếm
			if (!String.IsNullOrEmpty(searchString))
			{
				blogs = blogs.Where(b => b.Name.Contains(searchString));
			}

			// Áp dụng sắp xếp
			switch (sortOrder)
			{
				case "name_desc":
					blogs = blogs.OrderByDescending(b => b.Name);
					break;
				case "Url":
					blogs = blogs.OrderBy(b => b.Url);
					break;
				case "url_desc":
					blogs = blogs.OrderByDescending(b => b.Url);
					break;
				default:
					blogs = blogs.OrderBy(b => b.Name);
					break;
			}

			// Phân trang
			int pageSize = size ?? 5;
			int pageNumber = (page ?? 1);
			ViewBag.pageSize = new SelectList(new[] { "5", "10", "20", "25", "50", "100", "200" }, pageSize.ToString());

			return View(blogs.ToPagedList(pageNumber, pageSize));
		}

		// GET: Blogs/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogId,Name,Url")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        // GET: Blogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogId,Name,Url")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Blog blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
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
