using System.Web.Mvc;
using Common;
using DataTransferObjects;
using Models;
using Service.SystemManage;
using TelemarketingManagement.Base;


namespace TelemarketingManagement.Areas.SystemManage.Controllers
{

    public class CustomerCategoryController : BaseController
    {
        private readonly string _modelDescription;

        public CustomerCategoryController()
        {
            _modelDescription = typeof(CustomerCategory).GetEnumDescription();
        }


        public CustomerCategoryService CustomerCategoryService { get; set; }

        // GET: SystemManage/CustomerGenre
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetData(CustomerCategorySearchDto dto)
        {
            var rows = CustomerCategoryService.Search(dto);

            var result = new ResultInfo<CustomerCategory>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = rows
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var customerCategory = new CustomerCategory();

            //是否为新增
            if (id > 0)
            {
                customerCategory = CustomerCategoryService.GetDataById(id) ?? new CustomerCategory();
            }
            return View(customerCategory);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(CustomerCategoryEditDto dto)
        {
            CustomerCategoryService.Save(dto);
            return Json(new BaseResponseDto {Message = "保存成功！", Success = true, Title = _modelDescription});
        }


        public JsonResult Remove(params long[] id)
        {
            CustomerCategoryService.Remove(id);
            return Json(new BaseResponseDto {Message = "删除成功！", Success = true, Title = _modelDescription});
        }

    }

}