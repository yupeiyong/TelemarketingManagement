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

    public class LowerStatementReport2Controller : AbstractBaseController
    {

        public LowerStatementReportService LowerStatementReportService { get; set; }

        // GET: AdminManager/LowerStatementReport
        public ActionResult Index()
        {
            var model = new LowerStatementReportDetailsViewModel();
            return View(model);
        }


        public PartialViewResult ListPartial()
        {
            var reports = LowerStatementReportService.List();
            var model = new LowerStatementReportDetailsViewModel
            {
                LowerStatementReports = reports
            };

            return PartialView(model);
        }

        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateReportDto dto)
        {
            LowerStatementReportService.GenerateReport(dto);
            return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        }


        //public FileStreamResult Export()
        //{
        //    return LowerStatementReportService.Export();
        //}

        public JsonResult Export()
        {
            var fileName = $"{typeof(LowerStatementReport).GetDescription()}.xlsx";
            var fileStream = LowerStatementReportService.Export();
            var fileInfoDto = FilePoolHelper.FilePoolServer.Put(fileStream, fileName);

            return Json(new DownloadResponseDto { Success = true, Message = "导出报表成功！", DownloadFileUrl = fileInfoDto.RawFilename, OriginalFileName = fileInfoDto.OriginalFilename }, JsonRequestBehavior.AllowGet);
        }

    }

}