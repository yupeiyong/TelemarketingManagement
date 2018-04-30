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

    public class BudgetUnitReport2Controller : AbstractBaseController
    {

        public BudgetUnitReportService BudgetUnitReportService { get; set; }

        public string AppDataTempPathFullname { get; set; }


        public ActionResult Index()
        {
            var model = new BudgetUnitReportDetailsViewModel();
            return View(model);
        }


        public PartialViewResult ListPartial()
        {
            var reports = BudgetUnitReportService.List();
            var model = new BudgetUnitReportDetailsViewModel
            {
                BudgetUnitReports = reports
            };
            return PartialView(model);
        }


        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateReportDto dto)
        {
            BudgetUnitReportService.GenerateReport(dto);
            return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        }


        //public FileStreamResult Export()
        //{
        //    return BudgetUnitReportService.Export();
        //}


        public JsonResult Export()
        {
            var fileName = $"{typeof (BudgetUnitReport).GetDescription()}.xlsx";
            var fileStream = BudgetUnitReportService.Export();
            var fileInfoDto = FilePoolHelper.FilePoolServer.Put(fileStream, fileName);

            return Json(new DownloadResponseDto {Success = true, Message = "导出报表成功！", DownloadFileUrl = fileInfoDto.RawFilename, OriginalFileName = fileInfoDto.OriginalFilename}, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGenerateDateTime()
        {
            var generateDate = BudgetUnitReportService.GetGenerateDateTime();
            var dateString = generateDate?.ToString("yyyy-MM-dd HH:mm") ?? "";
            return Json(new BaseResponseDto {Success = true, Message = dateString}, JsonRequestBehavior.AllowGet);
        }

    }

}