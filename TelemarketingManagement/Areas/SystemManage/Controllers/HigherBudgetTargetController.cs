using System;
using System.Linq;
using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.Configs;
using JJsites.DataTransferObjects;
using JJsites.DomainModels;
using JJsites.ViewModels;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.DomainModels;
using JJsoft.Infrastructure.Extensions;
using JJsoft.Infrastructure.NetUtils;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class HigherBudgetTargetController : AbstractBaseController
    {

        private readonly string _modelDescription = typeof (HigherBudgetTarget).GetDescription();


        public HigherBudgetTargetService HigherBudgetTargetService { get; set; }

        public BasicDataService BasicDataService { get; set; }


        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetData(HigherBudgetTargetSearchDto dto)
        {
            dto.IsGettingTotalCount = true;

            var rows = HigherBudgetTargetService.Search(dto);
            var models = rows.CloneAnyMembersRange<HigherBudgetTarget, HigherBudgetTargetViewModel>();
            var result = new ResultInfo<HigherBudgetTargetViewModel>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = models
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var viewModel = new HigherBudgetTargetEditViewModel
            {
                HigherBudgetTarget = HigherBudgetTargetService.GetDataById(id) ?? new HigherBudgetTarget(),
                BasicDataBaseModels = BasicDataService.GetDefaultBaseModels()
            };
            return View(viewModel);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(HigherBudgetTargetEditDto dto)
        {
            HigherBudgetTargetService.Save(dto);
            return Json(new BaseResponseDto {Message = "保存成功！", Success = true, Title = _modelDescription});
        }


        public JsonResult Remove(params long[] id)
        {
            HigherBudgetTargetService.Remove(id);
            return Json(new BaseResponseDto {Message = "删除成功！", Success = true, Title = _modelDescription});
        }


        public JsonResult GetBasicData(BasicDataSearchDto dto)
        {
            var rows = BasicDataService.SearchBasicData(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> {IsSuccess = false, Message = "查找结果为0"}, JsonRequestBehavior.AllowGet);

            var result = new ResultInfo<SelectViewModel>
            {
                IsSuccess = true,
                Data = rows.Select(m => new SelectViewModel {id = m.Id, text = m.CustomedNumberAndName}).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTreeBasicData(BasicDataSearchDto dto)
        {
            dto.TakeCount = AppSettings.MaxTakeCount;
            var rows = BasicDataService.GetTreeBasicData(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> {IsSuccess = false, Message = "查找结果为0"}, JsonRequestBehavior.AllowGet);

            return Json(rows, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Print(long id = -1)
        //{
        //    var data = HigherBudgetTargetService.GetDataById(id) ?? new HigherBudgetTarget();
        //    return View(data);
        //}

        //导出
        public FileResult DoExport(HigherBudgetTargetSearchDto dto)
        {
            var stream = HigherBudgetTargetService.DoExport(dto);
            stream.Position = 0;
            var fileStreamResult = new FileStreamResult(stream, HttpContentTypeHelper.GetContentType(".xlsx")) {FileDownloadName = $"上级专款指标导出_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx"};
            return fileStreamResult;
        }

    }

}