using System.Web;
using System.Web.Mvc;

namespace nmct.project.web.itbedrijf
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
