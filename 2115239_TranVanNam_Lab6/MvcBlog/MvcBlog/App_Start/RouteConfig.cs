using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcBlog
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name: "Blogs",
				url: "Blogs/{action}/{id}",
				defaults: new { controller = "Blogs", action = "Index", id = UrlParameter.Optional }
			);
			routes.MapRoute(
				name:"Posts",
				url: "Posts/{action}/{id}",
				defaults: new { Controller ="Posts", action = "Index", id = UrlParameter.Optional}
				);

		}
	}
}
