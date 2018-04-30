using System.Web.Mvc;
using JJsites.BusinessLogicLayer.Reports;
using JJsites.DataTransferObjects.Reports;
using JJsites.ViewModels.Reports;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    /// <summary>
    ///     下级单位对帐汇总报表
    /// </summary>
    public class LowerStatementSummaryReportController : AbstractBaseController
    {

        public LowerStatementSummaryReportService LowerStatementSummaryReportService { get; set; }

        // GET: AdminManager/LowerStatementSummaryReport
        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult ListPartial(ReportSearchDto dto)
        {
            var model = LowerStatementSummaryReportService.Search(dto);
            return PartialView(model);
        }


        public ViewResult Details(GetLowerStatementSummaryReportDto dto)
        {
            var model = new LowerStatementSummaryReportDetailsViewModel {Title = $"没有找到指定的报表（Id：{dto.Id}）！"};
            var report = model.LowerStatementSummaryReport = LowerStatementSummaryReportService.GetByParameters(dto);
            if (report != null)
                model.Title = $"{report.Id}-{report.CreatedBy?.NickName}-{report.OnLastUpdated?.ToString("yyyy-MM-dd HH:mm:ss")}";

            return View(model);
        }


        public PartialViewResult DetailsListPartial(GetLowerStatementSummaryReportDto dto)
        {
            var model = new LowerStatementSummaryReportDetailsViewModel {Title = $"没有找到指定的报表（Id：{dto.Id}）！"};
            var report = model.LowerStatementSummaryReport = LowerStatementSummaryReportService.GetByParameters(dto);
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
        //    LowerStatementSummaryReportService.GenerateReport(CurrentUser.Id);
        //    return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        //}


        //public FileStreamResult Export(long id = -1)
        //{
        //    return LowerStatementSummaryReportService.Export(id);
        //}

    }

}