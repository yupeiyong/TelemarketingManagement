using System.Web.Mvc;
using Common.Operator;
using Common.Web;


namespace TelemarketingManagement.App_Start.Filters
{
    public class LoginAttribute : AuthorizeAttribute
    {
        public bool Ignore = true;
        public LoginAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Ignore == false)
            {
                return;
            }
            if (OnlineUserProvider.Provider.GetCurrent() == null)
            {
                WebHelper.WriteCookie("online_user_login_error", "overdue");
                filterContext.HttpContext.Response.Write("<script>top.location.href = '/Login/Index';</script>");
                return;
            }
        }
    }

}