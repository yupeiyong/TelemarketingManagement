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

    public class BudgetSourceReport2Controller : AbstractBaseController
    {

        public BudgetSourceReportService BudgetSourceReportService { get; set; }


        public ActionResult Index()
        {
            var model = new BudgetSourceReportDetailsViewModel();
            return View(model);
        }


        public PartialViewResult ListPartial()
        {
            var reports = BudgetSourceReportService.List();
            var model = new BudgetSourceReportDetailsViewModel
            {
                BudgetSourceReports = reports
            };
            return PartialView(model);
        }


        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateBudgetSourceReportDto dto)
        {
            BudgetSourceReportService.GenerateReports(dto);
            return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Export()
        {
            var fileName = $"{typeof (BudgetSourceReport).GetDescription()}.xlsx";
            var fileStream = BudgetSourceReportService.Export();
            var fileInfoDto = FilePoolHelper.FilePoolServer.Put(fileStream, fileName);

            var downloadResponseDto = new DownloadResponseDto
            {
                Message = "导出报表成功！"
                , DownloadFileUrl = fileInfoDto.RawFilename
                , OriginalFileName = fileInfoDto.OriginalFilename
            };
            return Json(downloadResponseDto, JsonRequestBehavior.AllowGet);
        }

    }

}