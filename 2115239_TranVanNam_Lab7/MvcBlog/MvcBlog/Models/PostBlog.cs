using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBlog.Models
{
	public class PostBlog
	{
		public int PostId { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int BlogId { get; set; }
		public string Name { get; set; }
	}
}