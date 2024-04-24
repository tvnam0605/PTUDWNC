using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using PagedList;

namespace MvcBlog.Controllers
{
    public class PostsController : Controller
    {
        private MvcBlogEntities db = new MvcBlogEntities();

        // GET: Posts
		[HttpGet]
        public ActionResult Index(string sortOrder, string title, int? page, int? size)
        {
			ViewBag.CurrentSort = sortOrder;
			ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
			ViewBag.ContentSortParm = sortOrder == "content_desc" ? "Content" : "content_desc";

			var posts = from p in db.Posts
						select p;
			#region sắp xếp
			// săp xếp
			switch (sortOrder)
			{
				case "title_desc":
					posts = posts.OrderByDescending(s => s.Title);
					break;
				case "Content":
					posts = posts.OrderBy(s => s.Content);
					break;
				case "content_desc":
					posts = posts.OrderByDescending(s => s.Content);
					break;
				default:
					posts = posts.OrderBy(s => s.Title);
					break;
			}
			#endregion
			#region tìm kiếm
			if (!String.IsNullOrEmpty(title))
			{
				posts = posts.Where(s => s.Title.Contains(title));
			}
			#endregion
			#region phân trang
			ViewBag.page = page;
			List<SelectListItem> items = new List<SelectListItem>();
			items.Add(new SelectListItem { Text = "5", Value = "5" });
			items.Add(new SelectListItem { Text = "10", Value = "10" });
			items.Add(new SelectListItem { Text = "20", Value = "20" });
			items.Add(new SelectListItem { Text = "25", Value = "25" });
			items.Add(new SelectListItem { Text = "50", Value = "50" });
			items.Add(new SelectListItem { Text = "100", Value = "100" });
			items.Add(new SelectListItem { Text = "200", Value = "200" });
			// 5.2. Nếu page = null thì đặt lại là 1.
			page = page ?? 1; //if (page == null) page = 1;

			// 5.3. Tạo kích thước trang (pageSize), mặc định là 5.
			int pageSize = (size ?? 5);

			ViewBag.pageSize = pageSize;

			// 6. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
			// nếu page = null thì lấy giá trị 1 cho biến pageNumber. --- dammio.com
			int pageNumber = (page ?? 1);

			// 6.2 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
			int checkTotal = (int)(posts.ToList().Count / pageSize) + 1;
			// Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
			if (pageNumber > checkTotal) pageNumber = checkTotal;
			#endregion
			// 7. Trả về các Link được phân trang theo kích thước và số trang.
			return View(posts.ToPagedList(pageNumber, pageSize));
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
