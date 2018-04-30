using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.DataTransferObjects;
using JJsites.DomainModels;
using JJsites.ViewModels;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BasicDataSearchController : AbstractBaseController
    {

        public BasicDataService BasicDataService { get; set; }

        public BasicDataTypeCustomedOrderService BasicDataTypeCustomedOrderService { get; set; }


        public ActionResult Index()
        {
            var model = new BasicDataSearchIndexViewModel
            {
                BasicDataTypeCustomedOrders = BasicDataTypeCustomedOrderService.List()
            };
            return View(model);
        }


        public JsonResult GetData(BasicDataSearchDto dto)
        {
            dto.IsGettingTotalCount = true;

            var rows = BasicDataService.SearchBasicData(dto);
            var result = new ResultInfo<BasicDataBaseModel>
            {
                IsSuccess = true
                , Total = dto.TotalCount
                , Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(BasicDataGetDataByIdDto dto)
        {
            var data = BasicDataService.GetDataById(dto);
            var viewModel = new BasicDataEditViewModel
            {
                ModelTypeName = dto.ModelTypeName,
                BasicData = data
            };

            return View(viewModel);
        }


        public ActionResult Add(BasicDataGetDataByIdDto dto)
        {
            var viewModel = new BasicDataEditViewModel
            {
                ModelTypeName=dto.ModelTypeName
            };
            return View("Edit",viewModel);
        }


        public JsonResult Save(BasicDataEditDto dto)
        {
            BasicDataService.Save(dto);
            return Json(new BaseResponseDto {Message = "保存成功！", Success = true, Title = "基础资料"});
        }


        public JsonResult Remove(BasicDataRemoveDto dto)
        {
            BasicDataService.Remove(dto);
            return Json(new BaseResponseDto {Message = "删除成功！", Success = true, Title = "基础资料"});
        }

    }

}