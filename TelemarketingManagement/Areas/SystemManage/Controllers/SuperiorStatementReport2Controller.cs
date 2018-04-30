using System.Web.Mvc;
using JJsites.BusinessLogicLayer.Reports;
using JJsites.DataTransferObjects;
using JJsites.DataTransferObjects.Reports;
using JJsites.DomainModels.Reports;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class SuperiorStatementReport2Controller : AbstractBaseController
    {

        public SuperiorStatementReportService SuperiorStatementReportService { get; set; }

        // GET: AdminManager/SuperiorStatementReport
        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult ListPartial()
        {
            var model = SuperiorStatementReportService.GetDetailsViewModel();
            return PartialView(model);
        }


        /// <summary>
        ///     生成报表
        /// </summary>
        /// <returns></returns>
        public JsonResult GenerateReport(GenerateSuperiorStatementReportDto dto)
        {
            SuperiorStatementReportService.GenerateReport(dto);
            return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        }


        //public FileStreamResult Export()
        //{
        //    return SuperiorStatementReportService.Export();
        //}

        public JsonResult Export()
        {
            var fileName = $"{typeof(SuperiorStatementReport).GetDescription()}.xlsx";
            var fileStream = SuperiorStatementReportService.Export();
            var fileInfoDto = FilePoolHelper.FilePoolServer.Put(fileStream, fileName);

            return Json(new DownloadResponseDto { Success = true, Message = "导出报表成功！", DownloadFileUrl = fileInfoDto.RawFilename, OriginalFileName = fileInfoDto.OriginalFilename }, JsonRequestBehavior.AllowGet);
        }

    }

}