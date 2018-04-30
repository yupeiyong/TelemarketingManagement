using Common;
using DataTransferObjects;
using Models;
using Models.Enum;
using Service;
using Service.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelemarketingManagement.App_Start.Base;
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
        //public FileResult DoExport(BudgetTargetSearchDto dto)
        //{
        //    var stream = BudgetTargetService.DoExport(dto);
        //    stream.Position = 0;
        //    var fileStreamResult = new FileStreamResult(stream, HttpContentTypeHelper.GetContentType(".xlsx")) { FileDownloadName = $"指标导出_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx" };
        //    return fileStreamResult;
        //}


        public JsonResult GetData(CustomerSearchDto dto)
        {
            dto.IsGetTotalCount = true;
            dto.PageSize = 20;
            var rows = CustomerService.Search(dto);
            //var viewModels = plans.MapToList<ChongQinSSCPlanViewModel>();
            //var models = rows.CloneAnyMembersRange<BudgetTarget, BudgetTargetViewModel>();
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
    }
}