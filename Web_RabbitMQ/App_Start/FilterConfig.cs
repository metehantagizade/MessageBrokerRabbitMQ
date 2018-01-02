using System.Web;
using System.Web.Mvc;

namespace Web_RabbitMQ
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
