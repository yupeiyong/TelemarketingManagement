using Common.Operator;
using DataTransferObjects;
using Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TelemarketingManagement.Controllers
{
    public class UserController : Controller
    {
        public UserService UserService { get; set; }
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }



        //登录
        public JsonResult LoginSubmitJson(UserLoginDto dto)
        {
            var currentOnlineUser = UserService.Login(dto);
            if (currentOnlineUser == null)
                throw new Exception("错误：用户登录失败，注册数据为空，请联系系统管理员！");

            var result = new LoginResultDto
            {
                Title = "用户登录",
                Message = "登录成功！",
                Success = true,
                NickName = currentOnlineUser.NickName,
                RedirectUrl= "/SystemManage/Home"
            };
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult OutLogin()
        {
            Session.Abandon();
            Session.Clear();
            OnlineUserProvider.Provider.RemoveCurrent();
            return RedirectToAction("Login");
        }

        //注销
        //public JsonResult LogoutSubmitJson()
        //{
        //    var currentOnlineUser = OnlineUserService.ProcessLogoutRequest(CurrentUser.Id, Token, ClientAddress);

        //    var currentUser = currentOnlineUser.User as AllUser;
        //    if (currentUser != null && currentUser.Id > 0)
        //        throw new Exception("系统错误：注销，操作失败！");

        //    Token = currentOnlineUser.Token;
        //    var result = new BaseResponseDto
        //    {
        //        Title = "用户注销",
        //        Message = "操作成功！",
        //        Success = true
        //    };
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}


        //注销
        //public ActionResult Logout()
        //{
        //    OnlineUserService.ProcessLogoutRequest(CurrentOnlineUser.Id, Token, ClientAddress);
        //    return Redirect("/");
        //}
    }
}