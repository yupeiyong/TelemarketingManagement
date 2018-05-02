using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelemarketingManagement.App_Start.Base;

namespace TelemarketingManagement.Areas.SystemManage.Controllers
{
    public class TelephoneRecordingController : BaseController
    {
        // GET: SystemManage/TelephoneRecording
        public ActionResult Index()
        {
            return View();
        }
    }
}