using Common;
using DataTransferObjects;
using Models;
using Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TelemarketingManagement.Base;
using ViewModels;

namespace TelemarketingManagement.Areas.SystemManage.Controllers
{
    public class TelephoneRecordingController : BaseController
    {

        private readonly string _modelDescription;

        public TelephoneRecordingController()
        {
            _modelDescription = typeof(TelephoneRecording).GetEnumDescription();
        }
        // GET: SystemManage/TelephoneRecording
        public ActionResult Index()
        {
            return View();
        }

        public TelephoneRecordingService TelephoneRecordingService { get; set; }

        public CustomerService CustomerService { get; set; }

        public UserService UserService { get; set; }
        public JsonResult GetData(TelephoneRecordingSearchDto dto)
        {
            var rows = TelephoneRecordingService.Search(dto);

            var result = new ResultInfo<TelephoneRecording>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var telephoneRecording = new TelephoneRecording();
            //是否为新增
            if (id > 0)
            {
                telephoneRecording = TelephoneRecordingService.GetDataById(id) ?? new TelephoneRecording();
            }
            return View(telephoneRecording);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(TelephoneRecordingEditDto dto)
        {
            dto.VisitorId = CurrentOnlineUser.UserId;
            TelephoneRecordingService.Save(dto);
            return Json(new BaseResponseDto { Message = "保存成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult Remove(params long[] id)
        {
            TelephoneRecordingService.Remove(id);
            return Json(new BaseResponseDto { Message = "删除成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult GetCustomers(CustomerSearchDto dto)
        {
            var rows = CustomerService.Search(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> { IsSuccess = false, Message = "查找结果为0" }, JsonRequestBehavior.AllowGet);

            var result = new ResultInfo<SelectViewModel>
            {
                IsSuccess = true,
                Data = rows.Select(m => new SelectViewModel { id = m.Id, text = m.RealName }).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetUsers(UserSearchDto dto)
        {
            var rows = UserService.Search(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> { IsSuccess = false, Message = "查找结果为0" }, JsonRequestBehavior.AllowGet);

            var result = new ResultInfo<SelectViewModel>
            {
                IsSuccess = true,
                Data = rows.Select(m => new SelectViewModel { id = m.Id, text = m.NickName }).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadFile()
        {
            var file = Request.Files[0];
            var dto = TelephoneRecordingFileHelper.Upload(file);
            return Json(dto, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BatchRemove(string ids)
        {

            if (string.IsNullOrWhiteSpace(ids))
                return Json(new BaseResponseDto { Success = false, Message = "删除内容为空！" }, JsonRequestBehavior.AllowGet);

            var idArray = JsonConvert.DeserializeObject<List<long>>(ids);
            try
            {
                TelephoneRecordingService.Remove(idArray.ToArray());
                return Json(new BaseResponseDto { Message = $"成功删除{idArray.Count}条电话录音记录！", Success = true, Title = _modelDescription }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new BaseResponseDto { Message = $"删除录音记录失败！" + ex.Message, Success = false, Title = _modelDescription }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}