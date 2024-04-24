using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcBlog.Models;
using System.Linq.Dynamic.Core; 
using PagedList;

using System.Linq.Expressions;
using System.Reflection;

namespace MvcBlog.Controllers
{
    public class BlogsController : Controller
    {
        private MvcBlogEntities db = new MvcBlogEntities();

		// GET: Blogs
		[HttpGet]
		public ActionResult Index(string sortOrder, string name, string des, string owner, int? page, int? size)
		{
			ViewBag.CurrentSort = sortOrder;
			ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewBag.DescriptionSortParm = sortOrder == "description_desc" ? "Description" : "description_desc";
			ViewBag.OwnerSortParm = sortOrder == "owner_desc" ? "Owner" : "owner_desc";
			
			var blogs = from s in db.Blogs
						select s;
			var owners = db.Blogs.Select(b => b.Owner).Distinct().OrderBy(o => o).ToList();

			ViewBag.OwnerList = new SelectList(owners, "Owner");
			ViewBag.page = page;
			List<SelectListItem> items = new List<SelectListItem>();
			items.Add(new SelectListItem { Text = "5", Value = "5" });
			items.Add(new SelectListItem { Text = "10", Value = "10" });
			items.Add(new SelectListItem { Text = "20", Value = "20" });
			items.Add(new SelectListItem { Text = "25", Value = "25" });
			items.Add(new SelectListItem { Text = "50", Value = "50" });
			items.Add(new SelectListItem { Text = "100", Value = "100" });
			items.Add(new SelectListItem { Text = "200", Value = "200" });
			foreach (var item in items)
			{
				if (item.Value == size.ToString()) item.Selected = true;
			}
			ViewBag.size = items;
			ViewBag.currentSize = size;
			switch (sortOrder)
			{
				case "name_desc":
					blogs = blogs.OrderByDescending(s => s.Name);
					break;
				case "Description":
					blogs = blogs.OrderBy(s => s.Description);
					break;
				case "description_desc":
					blogs = blogs.OrderByDescending(s => s.Description);
					break;
				case "Owner":
					blogs = blogs.OrderBy(s => s.Owner);
					break;
				case "owner_desc":
					blogs = blogs.OrderByDescending(s => s.Owner);
					break;
				default:  // Name ascending 
					blogs = blogs.OrderBy(s => s.Name);
					break;
			}
			if (!String.IsNullOrEmpty(name))
			{
				blogs = blogs.Where(s => s.Name.Contains(name));
			}

			if (!String.IsNullOrEmpty(des))
			{
				blogs = blogs.Where(s => s.Description.Contains(des));
			}

			if (!String.IsNullOrEmpty(owner))
			{
				blogs = blogs.Where(s => s.Owner == owner);
			}

			// Chuyển đổi sang IPagedList trước khi truyền sang View

			// 5.2. Nếu page = null thì đặt lại là 1.
			page = page ?? 1; //if (page == null) page = 1;

			// 5.3. Tạo kích thước trang (pageSize), mặc định là 5.
			int pageSize = (size ?? 5);

			ViewBag.pageSize = pageSize;

			// 6. Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
			// nếu page = null thì lấy giá trị 1 cho biến pageNumber. --- dammio.com
			int pageNumber = (page ?? 1);

			// 6.2 Lấy tổng số record chia cho kích thước để biết bao nhiêu trang
			int checkTotal = (int)(blogs.ToList().Count / pageSize) + 1;
			// Nếu trang vượt qua tổng số trang thì thiết lập là 1 hoặc tổng số trang
			if (pageNumber > checkTotal) pageNumber = checkTotal;

			// 7. Trả về các Link được phân trang theo kích thước và số trang.
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
        public ActionResult Create([Bind(Include = "BlogId,Name,Description,Owner")] Blog blog)
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
        public ActionResult Edit([Bind(Include = "BlogId,Name,Description,Owner")] Blog blog)
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
