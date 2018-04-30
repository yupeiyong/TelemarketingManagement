using System.Linq;
using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.Configs;
using JJsites.DataTransferObjects;
using JJsites.ViewModels;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BasicDataController : AbstractBaseController
    {

        public BasicDataService BasicDataService { get; set; }


        public JsonResult GetBasicData(BasicDataSearchDto dto)
        {
            var rows = BasicDataService.SearchBasicData(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> {IsSuccess = false, Message = "查找结果为0"}, JsonRequestBehavior.AllowGet);

            var result = new ResultInfo<SelectViewModel>
            {
                IsSuccess = true,
                Data = rows.Select(m => new SelectViewModel {id = m.Id, text = m.CustomedNumberAndName}).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetTreeBasicData(BasicDataSearchDto dto)
        {
            dto.TakeCount = AppSettings.MaxTakeCount;
            var rows = BasicDataService.GetTreeBasicData(dto);
            if (rows == null || rows.Count == 0)
                return Json(new ResultInfo<SelectViewModel> {IsSuccess = false, Message = "查找结果为0"}, JsonRequestBehavior.AllowGet);

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

    }

}