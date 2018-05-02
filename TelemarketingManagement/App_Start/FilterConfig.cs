using System.Web.Mvc;
using TelemarketingManagement.App_Start.Filters;

namespace TelemarketingManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new VerificationAttribute());

        }
    }
}
