using System.Web.Mvc;
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

    /// <summary>
    ///     下级单位对帐汇总报表
    /// </summary>
    public class LowerStatementSummaryReport2Controller : AbstractBaseController
    {

        public LowerStatementSummaryReportService LowerStatementSummaryReportService { get; set; }

        // GET: AdminManager/LowerStatementSummaryReport
        public ActionResult Index()
        {
            var report = LowerStatementSummaryReportService.GetReport();
            var model = new LowerStatementSummaryReportDetailsViewModel
            {
                LowerStatementSummaryReport = report
            };
            return View(model);
        }


        public PartialViewResult ListPartial()
        {
            var report = LowerStatementSummaryReportService.GetReport();
            var model = new LowerStatementSummaryReportDetailsViewModel
            {
                LowerStatementSummaryReport = report
            };
            return PartialView(model);
        }


        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateReportDto dto)
        {
            LowerStatementSummaryReportService.GenerateReport(dto);
            return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Export()
        {
            var fileName = $"{typeof(LowerStatementSummaryReport).GetDescription()}.xlsx";
            var fileStream = LowerStatementSummaryReportService.Export();
            var fileInfoDto = FilePoolHelper.FilePoolServer.Put(fileStream, fileName);

            return Json(new DownloadResponseDto { Success = true, Message = "导出报表成功！", DownloadFileUrl = fileInfoDto.RawFilename, OriginalFileName = fileInfoDto.OriginalFilename }, JsonRequestBehavior.AllowGet);
        }

    }

}