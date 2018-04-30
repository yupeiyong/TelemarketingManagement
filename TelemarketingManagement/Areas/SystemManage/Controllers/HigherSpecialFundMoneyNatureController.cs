using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.DataTransferObjects;
using JJsites.DomainModels;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class HigherSpecialFundMoneyNatureController : AbstractBaseController
    {

        private readonly string _modelDescription = typeof (HigherSpecialFundMoneyNature).GetDescription();

        public HigherSpecialFundMoneyNatureService TargetNatureService { get; set; }


        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetData(HigherSpecialFundMoneyNatureSearchDto dto)
        {
            dto.IsGettingTotalCount = true;

            var rows = TargetNatureService.Search(dto);
            var result = new ResultInfo<HigherSpecialFundMoneyNature>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var data = TargetNatureService.GetDataById(id);
            return View(data);
        }


        public ActionResult Add()
        {
            return View("Edit");
        }


        public JsonResult Save(HigherSpecialFundMoneyNatureEditDto dto)
        {
            TargetNatureService.Save(dto);
            return Json(new BaseResponseDto {Message = "保存成功！", Success = true, Title = _modelDescription});
        }


        public JsonResult Remove(params long[] id)
        {
            TargetNatureService.Remove(id);
            return Json(new BaseResponseDto {Message = "删除成功！", Success = true, Title = _modelDescription});
        }

    }

}