using Common;
using Common.Configs;
using Config;
using DataTransferObjects;
using Models;
using Models.Enum;
using Service;
using Service.SystemManage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelemarketingManagement.App_Start.Base;
using TelemarketingManagement.Base;
using ViewModels;

namespace TelemarketingManagement.Areas.SystemManage.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly string _modelDescription;

        public CustomerController()
        {
            _modelDescription = typeof(Customer).GetEnumDescription();
        }
        // GET: SystemManage/Customer
        public ActionResult Index()
        {
            return View();
        }

        public CustomerService CustomerService { get; set; }

        public CustomerCategoryService CustomerCategoryService { get; set; }



        public JsonResult GetData(CustomerSearchDto dto)
        {
            var rows = CustomerService.Search(dto);

            var result = new ResultInfo<Customer>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var customer = new Customer();
            //是否为新增
            if (id >0)
            {
                customer = CustomerService.GetDataById(id) ?? new Customer();
            }
            var viewModel = new CustomerEditViewModel
            {
               Customer= customer,
               Genders=Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList()
            };
            return View(viewModel);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(CustomerEditDto dto)
        {
            CustomerService.Save(dto);
            return Json(new BaseResponseDto { Message = "保存成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult Remove(params long[] id)
        {
            CustomerService.Remove(id);
            return Json(new BaseResponseDto { Message = "删除成功！", Success = true, Title = _modelDescription });
        }

        public JsonResult GetCategories(CustomerCategorySearchDto dto)
        {
            var rows = CustomerCategoryService.Search(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> { IsSuccess = false, Message = "查找结果为0" }, JsonRequestBehavior.AllowGet);

            var result = new ResultInfo<SelectViewModel>
            {
                IsSuccess = true,
                Data = rows.Select(m => new SelectViewModel { id = m.Id, text = m.Name }).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileStreamResult DownLoadTemplate()
        {
            var fileName = "客户资料模板.xls";
            var fullName = Path.Combine(Settings.AppDataPathFullname, $".Template/{fileName}");
            if (System.IO.File.Exists(fullName))
            {
                var fileResult = new FileStreamResult(new FileStream(fullName, FileMode.Open, FileAccess.Read, FileShare.Read), "application/ms-excel") { FileDownloadName = fileName };
                return fileResult;
            }
            throw new Exception("模板文件不存在！");
        }


        public JsonResult ImportData(ImportedFileDto dto)
        {
            var importDtos = CustomerFileHelper.DoImport(dto);
            var result=CustomerService.Import(importDtos);
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Import()
        {
            return View();
        }

        public JsonResult Upload()
        {
            var file = Request.Files[0];
            //var dtoes = ImportService.Upload(postedFiles);
            var dto = CustomerFileHelper.Upload(file);
            return Json(dto, JsonRequestBehavior.AllowGet);
        }
    }
}