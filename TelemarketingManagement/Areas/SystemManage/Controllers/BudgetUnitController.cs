using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.DataTransferObjects;
using JJsites.DomainModels;
using JJsites.ViewModels;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.DomainModels;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BudgetUnitController : AbstractBaseController
    {

        private readonly string _modelDescription = typeof (BudgetUnit).GetDescription();

        public BudgetUnitService BudgetUnitService { get; set; }

        public BudgetUnitLevelService BudgetUnitLevelService { get; set; }


        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetData(BudgetUnitSearchDto dto)
        {
            dto.IsGettingTotalCount = true;

            var rows = BudgetUnitService.Search(dto);
            var list = rows.CloneMembersRange<BudgetUnit, BudgetUnitViewModel>();
            var result = new ResultInfo<BudgetUnitViewModel>
            {
                IsSuccess = true
                , Total = dto.TotalCount
                , Data = list
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var data = BudgetUnitService.GetDataById(id);
            ViewBag.BudgetUnitLevels = BudgetUnitLevelService.List();
            return View(data);
        }


        public ActionResult Add()
        {
            ViewBag.BudgetUnitLevels = BudgetUnitLevelService.List();
            return View("Edit");
        }


        public JsonResult Save(BudgetUnitEditDto dto)
        {
            BudgetUnitService.Save(dto);
            return Json(new BaseResponseDto {Message = "保存成功！", Success = true, Title = _modelDescription});
        }


        public JsonResult Remove(params long[] id)
        {
            BudgetUnitService.Remove(id);
            return Json(new BaseResponseDto {Message = "删除成功！", Success = true, Title = _modelDescription});
        }

    }

}