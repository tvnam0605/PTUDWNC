using System.Web;
using System.Web.Mvc;

namespace PC112_2115239_TranVanNam
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
