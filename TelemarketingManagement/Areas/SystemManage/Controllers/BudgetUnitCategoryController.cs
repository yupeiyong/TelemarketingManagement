using System;
using System.Linq;
using System.Web.Mvc;
using JJsites.Enumerations;
using JJsites.ViewModels;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BudgetUnitCategoryController : AbstractBaseController
    {

        /// <summary>
        ///     获取单位性质列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBudgetUnitCategories()
        {
            var budgetUnitCategories = Enum.GetValues(typeof (BudgetUnitCategoryEnum)).Cast<BudgetUnitCategoryEnum>().ToList();

            var viewModels = budgetUnitCategories.Select(category => new EasyuiTreeViewModel
            {
                id = category,
                text = category.GetDescription(),
                Number = category.ToString(),
                state = ""
            }).ToList();
            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }

    }

}