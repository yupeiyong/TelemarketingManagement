using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DataTransferObjects;
using Models;
using Models.Enum;
using Service.SystemManage;
using TelemarketingManagement.Base;
using ViewModels;


namespace TelemarketingManagement.Areas.SystemManage.Controllers
{
    public class UserController : BaseController
    {
        private readonly string _modelDescription;

        public UserController()
        {
            _modelDescription = typeof(User).GetEnumDescription();
        }

        // GET: SystemManage/User
        public ActionResult Index()
        {
            return View();
        }


        public UserService UserService { get; set; }


        public JsonResult GetData(UserSearchDto dto)
        {
            var rows = UserService.Search(dto);

            var result = new ResultInfo<User>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var user = new User();
            //是否为新增
            if (id > 0)
            {
                user = UserService.GetDataById(id) ?? new User();
            }
            var viewModel = new UserEditViewModel
            {
                User = user,
                UserStates = Enum.GetValues(typeof(UserStateEnum)).Cast<UserStateEnum>().ToList(),
                Genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList()
            };
            return View(viewModel);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(UserEditDto dto)
        {
            UserService.Save(dto);
            return Json(new BaseResponseDto { Message = "保存成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult Remove(params long[] id)
        {
            UserService.Remove(id);
            return Json(new BaseResponseDto { Message = "删除成功！", Success = true, Title = _modelDescription });
        }
    }
}