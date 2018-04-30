using System.Web.Mvc;
using JJsites.BusinessLogicLayer.Reports;
using JJsites.DataTransferObjects.Reports;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BudgetUnitReportController : AbstractBaseController
    {

        public BudgetUnitReportService BudgetUnitReportService { get; set; }

        // GET: AdminManager/BudgetUnitFunctionSubjectReport
        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult ListPartial(ReportSearchDto dto)
        {
            var model = BudgetUnitReportService.Search(dto);
            return PartialView(model);
        }


        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateReportDto dto)
        {
            BudgetUnitReportService.GenerateReport(dto);
            return Json(new BaseResponseDto { Success = true, Message = "生成成功" }, JsonRequestBehavior.AllowGet);
        }


        //public ViewResult Details(GetBudgetUnitReportDto dto)
        //{
        //    var model = new BudgetUnitReportDetailsViewModel {Title = $"没有找到指定的报表（Id：{dto.Id}）！"};
        //    var report = model.BudgetUnitReport = BudgetUnitReportService.GetByParameters(dto);
        //    if (report != null)
        //        model.Title = $"{report.Id}-{report.CreatedBy?.NickName}-{report.OnLastUpdated?.ToString("yyyy-MM-dd HH:mm:ss")}";

        //    return View(model);
        //}

        //public PartialViewResult DetailsListPartial(GetBudgetUnitReportDto dto)
        //{
        //    var model = new BudgetUnitReportDetailsViewModel { Title = $"没有找到指定的报表（Id：{dto.Id}）！" };
        //    var report = model.BudgetUnitReport = BudgetUnitReportService.GetByParameters(dto);
        //    if (report != null)
        //        model.Title = $"{report.Id}-{report.CreatedBy?.NickName}-{report.OnLastUpdated?.ToString("yyyy-MM-dd HH:mm:ss")}";

        //    return PartialView(model);
        //}

        //public FileStreamResult Export()
        //{
        //    return BudgetUnitReportService.Export();
        //}

    }

}