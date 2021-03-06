﻿using System.Web.Mvc;
using Common.Log;
using Common.Operator;
using TelemarketingManagement.App_Start.Filters;


namespace TelemarketingManagement.Base
{
    [Login]
    public abstract class BaseController : Controller
    {
        public Log FileLog => LogFactory.GetLogger(this.GetType().ToString());

        public static OnlineUser CurrentOnlineUser { get; set; }
    }

}