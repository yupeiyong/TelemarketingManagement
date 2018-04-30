using System.Web.Mvc;
using JJsites.BusinessLogicLayer.Reports;
using JJsites.DataTransferObjects.Reports;
using JJsites.ViewModels.Reports;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BudgetSourceSummaryReportController : AbstractBaseController
    {

        public BudgetSourceSummaryReportService BudgetSourceSummaryReportService { get; set; }


        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult ListPartial(ReportSearchDto dto)
        {
            var model = BudgetSourceSummaryReportService.Search(dto);
            return PartialView(model);
        }


        public ViewResult Details(GetBudgetSourceSummaryReportDto dto)
        {
            var model = new BudgetSourceSummaryReportDetailsViewModel {Title = $"没有找到指定的报表（Id：{dto.Id}）！"};
            var report = model.BudgetSourceSummaryReport = BudgetSourceSummaryReportService.GetByParameters(dto);
            if (report != null)
                model.Title = $"{report.Id}-{report.CreatedBy?.NickName}-{report.OnLastUpdated?.ToString("yyyy-MM-dd HH:mm:ss")}";

            return View(model);
        }


        public PartialViewResult DetailsListPartial(GetBudgetSourceSummaryReportDto dto)
        {
            var model = new BudgetSourceSummaryReportDetailsViewModel {Title = $"没有找到指定的报表（Id：{dto.Id}）！"};
            var report = model.BudgetSourceSummaryReport = BudgetSourceSummaryReportService.GetByParameters(dto);
            if (report != null)
                model.Title = $"{report.Id}-{report.CreatedBy?.NickName}-{report.OnLastUpdated?.ToString("yyyy-MM-dd HH:mm:ss")}";

            return PartialView(model);
        }


        ///// <summary>
        /////     生成报表
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult GenerateReport()
        //{
        //    BudgetSourceSummaryReportService.GenerateReport(CurrentUser.Id);
        //    return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        //}


        //public FileStreamResult Export()
        //{
        //    return BudgetSourceSummaryReportService.Export();
        //}

    }

}