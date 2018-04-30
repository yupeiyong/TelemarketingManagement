using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.DomainModels;
using JJsites.Enumerations;
using JJsites.ViewModels;
using JJsoft.Infrastructure.Extensions;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    /// <summary>
    ///     政府经济科目
    /// </summary>
    public class CountryEconomicsSubjectController : AbstractBaseController
    {
        public CountryEconomicsSubjectService CountryEconomicsSubjectService { get; set; }

        /// <summary>
        ///     获取国家统一经济科目
        /// </summary>
        /// <param name="subjectId">经济科目Id</param>
        /// <param name="category"></param>
        /// <returns></returns>
        public JsonResult GetCountryEconomicsSubject(long subjectId, int category)
        {
            var result = CountryEconomicsSubjectService.GetDataByEconomicsSubjectNumberAndCategory(subjectId, (BudgetUnitCategoryEnum)category);

            var text = result?.CustomedNumberAndName ?? $"没有找到{typeof(CountryEconomicsSubject).GetDescription()}（{subjectId}, {category.GetDescription()}）";
            var viewModel = new EasyuiTreeViewModel
            {
                id = result?.Id,
                text = text,
                Number = category.ToString(),
                state = ""
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     通过政府经济科目和一般经济科目获取映射表中的单位性质
        /// </summary>
        /// <param name="economicsSubjectId">经济科目Id</param>
        /// <param name="countryEconomicsSubjectId"></param>
        /// <returns></returns>
        public BudgetUnitCategoryEnum GetBudgetUnitCategoryEnum(long economicsSubjectId, long countryEconomicsSubjectId)
        {
            return CountryEconomicsSubjectService.GetBudgetUnitCategoryEnum(economicsSubjectId, countryEconomicsSubjectId);
        }
    }

}