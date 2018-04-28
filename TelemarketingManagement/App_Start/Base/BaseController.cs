using Common.Operator;
using Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelemarketingManagement.App_Start.Filters;

namespace TelemarketingManagement.App_Start.Base
{
    [Login]
    public abstract class BaseController : Controller
    {
        public Log FileLog
        {
            get { return LogFactory.GetLogger(this.GetType().ToString()); }
        }
        public OnlineUser CurrentOnlineUser { get; set; }
        //[HttpGet]
        //[Authorize]
        //public virtual ActionResult Index()
        //{
        //    return View();
        //}
        //[HttpGet]
        //[Authorize]
        //public virtual ActionResult Form()
        //{
        //    return View();
        //}
        //[HttpGet]
        //[Authorize]
        //public virtual ActionResult Details()
        //{
        //    return View();
        //}
        //protected virtual ActionResult Success(string message)
        //{
        //    return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        //}
        //protected virtual ActionResult Success(string message, object data)
        //{
        //    return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        //}
        //protected virtual ActionResult Error(string message)
        //{
        //    return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        //}
    }

}