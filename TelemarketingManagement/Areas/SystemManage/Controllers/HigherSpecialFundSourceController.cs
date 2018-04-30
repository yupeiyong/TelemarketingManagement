using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.DataTransferObjects;
using JJsites.DomainModels;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class HigherSpecialFundSourceController : AbstractBaseController
    {

        private readonly string _modelDescription = typeof (HigherSpecialFundSource).GetDescription();

        public HigherSpecialFundSourceService HigherSpecialFundSourceService { get; set; }


        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetData(HigherSpecialFundSourceSearchDto dto)
        {
            dto.IsGettingTotalCount = true;

            var rows = HigherSpecialFundSourceService.Search(dto);
            var result = new ResultInfo<HigherSpecialFundSource>
            {
                IsSuccess = true
                , Total = dto.TotalCount
                , Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var data = HigherSpecialFundSourceService.GetDataById(id);
            return View(data);
        }


        public ActionResult Add()
        {
            return View("Edit");
        }


        public JsonResult Save(HigherSpecialFundSourceEditDto dto)
        {
            HigherSpecialFundSourceService.Save(dto);
            return Json(new BaseResponseDto {Message = "保存成功！", Success = true, Title = _modelDescription});
        }


        public JsonResult Remove(params long[] id)
        {
            HigherSpecialFundSourceService.Remove(id);
            return Json(new BaseResponseDto {Message = "删除成功！", Success = true, Title = _modelDescription});
        }

    }

}