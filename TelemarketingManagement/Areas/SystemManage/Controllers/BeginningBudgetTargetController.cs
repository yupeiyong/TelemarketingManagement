using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JJsites.BusinessLogicLayer;
using JJsites.DataTransferObjects;
using JJsites.DomainModels;
using JJsites.ViewModels;
using JJsoft.Infrastructure.DataTransferObjects;
using JJsoft.Infrastructure.DomainModels;
using JJsoft.Infrastructure.Extensions;
using JJsoft.Infrastructure.NetUtils;
using JJsoft.WebInfrastructure.AspMvcUtils;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BeginningBudgetTargetController : AbstractBaseController
    {

        private readonly string _modelDescription = typeof(BeginningBudgetTarget).GetDescription();

        public BeginningBudgetTargetService BeginningBudgetTargetService { get; set; }

        public BasicDataService BasicDataService { get; set; }


        public ActionResult Index()
        {
            return View();
        }


        public FileResult DoExport(BeginningBudgetTargetSearchDto dto)
        {
            var stream = BeginningBudgetTargetService.DoExport(dto);
            stream.Position = 0;
            var fileStreamResult = new FileStreamResult(stream, HttpContentTypeHelper.GetContentType(".xlsx")) { FileDownloadName = $"年初指标导出_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx" };
            return fileStreamResult;
        }


        public JsonResult GetData(BeginningBudgetTargetSearchDto dto)
        {
            dto.IsGettingTotalCount = true;
            dto.Keywords = dto.LikeWords;
            var rows = BeginningBudgetTargetService.Search(dto);
            var models = rows.CloneAnyMembersRange<BeginningBudgetTarget, BeginningBudgetTargetViewModel>();
            var result = new ResultInfo<BeginningBudgetTargetViewModel>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = models
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var beginningBudgetTarget = new BeginningBudgetTarget();
            var basicDataBaseModels = new Dictionary<string, BasicDataBaseModel>();

            //是否为新增
            if (id == -1)
            {
                basicDataBaseModels = BasicDataService.GetDefaultBaseModels();
            }
            else
            {
                beginningBudgetTarget = BeginningBudgetTargetService.GetDataById(id) ?? new BeginningBudgetTarget();
            }
            var viewModel = new BeginningBudgetTargetEditViewModel
            {
                BeginningBudgetTarget = beginningBudgetTarget,
                BasicDataBaseModels = basicDataBaseModels
            };
            return View(viewModel);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(BeginningBudgetTargetEditDto dto)
        {
            BeginningBudgetTargetService.Save(dto);
            return Json(new BaseResponseDto { Message = "保存成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult Remove(params long[] id)
        {
            BeginningBudgetTargetService.Remove(id);
            return Json(new BaseResponseDto { Message = "删除成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult Unlock(long id = -1)
        {
            BeginningBudgetTargetService.Unlock(id);
            return Json(new BaseResponseDto { Message = "解除锁定成功！", Success = true, Title = _modelDescription });
        }

    }

}