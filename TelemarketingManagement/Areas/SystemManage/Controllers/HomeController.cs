using System.Web.Mvc;
using TelemarketingManagement.Base;


namespace TelemarketingManagement.Areas.SystemManage.Controllers
{

    public class HomeController : BaseController
    {

        //public MenuService MenuService { get; set; }


        public ViewResult Index()
        {
            return View();
        }

    }

}