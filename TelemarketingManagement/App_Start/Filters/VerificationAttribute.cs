using Common.Operator;
using Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TelemarketingManagement.App_Start.Filters
{

    public class VerificationAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
        public VerificationAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var current = OnlineUserProvider.Provider.GetCurrent();
            if (current == null) return;
            if (current.IsSystem)
            {
                return;
            }
            if (Ignore == false)
            {
                return;
            }
            if (!this.ActionAuthorize(filterContext))
            {
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');</script>");
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var currentOnlineUser = OnlineUserProvider.Provider.GetCurrent();
            var roleId = currentOnlineUser.RoleId;
            var moduleId = WebHelper.GetCookie("system_currentmoduleid");
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            //return new RoleAuthorizeService().ActionValidate(roleId, moduleId, action);
            return true;
        }
    }

}
