using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.BusinessLogicLayer.Reports;
using JJsites.DataTransferObjects;
using JJsites.DataTransferObjects.Reports;
using JJsites.DomainModels.Reports;
using JJsites.ViewModels.Reports;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BudgetSourceSummaryReport2Controller : AbstractBaseController
    {

        public BudgetSourceSummaryReportService BudgetSourceSummaryReportService { get; set; }

        public BudgetUnitLevelService BudgetUnitLevelService { get; set; }


        public ActionResult Index()
        {
            var budgetUnitLevels = BudgetUnitLevelService.List();
            var model = new BudgetSourceSummaryReportDetailsViewModel
            {
                BudgetUnitLevels = budgetUnitLevels
            };

            return View(model);
        }


        public PartialViewResult ListPartial(ReportSearchDto dto)
        {
            var reports = BudgetSourceSummaryReportService.List();
            var model = new BudgetSourceSummaryReportDetailsViewModel
            {
                BudgetSourceSummaryReports = reports
            };
            return PartialView(model);
        }


        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateBudgetSourceReportDto dto)
        {
            BudgetSourceSummaryReportService.GenerateReport(dto);
            return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        }


        //public FileStreamResult Export()
        //{
        //    return BudgetSourceSummaryReportService.Export();
        //}

        public JsonResult Export()
        {
            var fileName = $"{typeof(BudgetSourceSummaryReport).GetDescription()}.xlsx";
            var fileStream = BudgetSourceSummaryReportService.Export();
            var fileInfoDto = FilePoolHelper.FilePoolServer.Put(fileStream, fileName);

            return Json(new DownloadResponseDto { Success = true, Message = "导出报表成功！", DownloadFileUrl = fileInfoDto.RawFilename, OriginalFileName = fileInfoDto.OriginalFilename }, JsonRequestBehavior.AllowGet);
        }
    }

}