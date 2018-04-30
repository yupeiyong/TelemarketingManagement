using System.Web.Mvc;
using JJsites.BusinessLogicLayer.Reports;
using JJsites.DataTransferObjects.Reports;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class SuperiorStatementReportController : AbstractBaseController
    {

        public SuperiorStatementReportService SuperiorStatementReportService { get; set; }

        // GET: AdminManager/SuperiorStatementReport
        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult ListPartial(ReportSearchDto dto)
        {
            var model = SuperiorStatementReportService.Search(dto);
            return PartialView(model);
        }


        public ViewResult Details(GetSuperiorStatementReportDto dto)
        {
            var model = SuperiorStatementReportService.GetDetailsViewModel();
            return View(model);
        }


        //public PartialViewResult DetailsListPartial(GetSuperiorStatementReportDto dto)
        //{
        //    var model = SuperiorStatementReportService.GetDetailsViewModel(dto);
        //    return PartialView(model);
        //}


        ///// <summary>
        /////     生成报表
        ///// </summary>
        ///// <returns></returns>
        //public JsonResult GenerateReport()
        //{
        //    SuperiorStatementReportService.GenerateReport(CurrentUser.Id);
        //    return Json(new BaseResponseDto {Success = true, Message = "生成成功"}, JsonRequestBehavior.AllowGet);
        //}


        //public FileStreamResult Export(long id = -1)
        //{
        //    return SuperiorStatementReportService.Export(id);
        //}

    }

}