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
using Newtonsoft.Json;


namespace JJsites.WebPc.Areas.AdminManager.Controllers
{

    public class BudgetTargetController : AbstractBaseController
    {

        private readonly string _modelDescription = typeof(BudgetTarget).GetDescription();

        public BudgetTargetService BudgetTargetService { get; set; }

        public BasicDataService BasicDataService { get; set; }


        /// <summary>
        ///     打印页面宽度px
        /// </summary>
        public int PrintPaperWidth { get; set; } = 676;


        /// <summary>
        ///     打印页面高度px
        /// </summary>
        public int PrintPaperHeight { get; set; } = 258;


        public ActionResult Index()
        {
            return View();
        }


        public FileResult DoExport(BudgetTargetSearchDto dto)
        {
            var stream = BudgetTargetService.DoExport(dto);
            stream.Position = 0;
            var fileStreamResult = new FileStreamResult(stream, HttpContentTypeHelper.GetContentType(".xlsx")) { FileDownloadName = $"指标导出_{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}.xlsx" };
            return fileStreamResult;
        }


        public JsonResult GetData(BudgetTargetSearchDto dto)
        {
            dto.IsGettingTotalCount = true;

            var rows = BudgetTargetService.Search(dto);
            var models = rows.CloneAnyMembersRange<BudgetTarget, BudgetTargetViewModel>();
            var result = new ResultInfo<BudgetTargetViewModel>
            {
                IsSuccess = true,
                Total = dto.TotalCount,
                Data = models
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(long id = -1)
        {
            var budgetTarget = new BudgetTarget();
            var basicDataBaseModels = new Dictionary<string, BasicDataBaseModel>();

            //是否为新增
            if (id == -1)
            {
                basicDataBaseModels = BasicDataService.GetDefaultBaseModels();
            }
            else
            {
                budgetTarget = BudgetTargetService.GetDataById(id) ?? new BudgetTarget();
            }
            var viewModel = new BudgetTargetEditViewModel
            {
                BudgetTarget = budgetTarget,
                BasicDataBaseModels = basicDataBaseModels
            };
            return View(viewModel);
        }


        public ActionResult Add()
        {
            return RedirectToAction("Edit");
        }


        public JsonResult Save(BudgetTargetEditDto dto)
        {
            BudgetTargetService.Save(dto);
            return Json(new BaseResponseDto { Message = "保存成功！", Success = true, Title = _modelDescription });
        }


        public JsonResult Remove(params long[] id)
        {
            BudgetTargetService.Remove(id);
            return Json(new BaseResponseDto { Message = "删除成功！", Success = true, Title = _modelDescription });
        }


        public ActionResult Print(long id = -1)
        {
            var target = BudgetTargetService.GetDataById(id) ?? new BudgetTarget();
            var parentFunctionSubjects = BasicDataService.GetParents(target.FunctionSubject);
            parentFunctionSubjects.Sort();
            var viewModel = target.CloneAnyMembers<BudgetTarget, BudgetTargetPrintViewModel>();
            viewModel.FunctionSubjects = parentFunctionSubjects;

            //锁定指标，不能再编辑
            if (!target.IsLocked)
                BudgetTargetService.Lock(id);

            var printViewModel = new PrintViewModel<BudgetTargetPrintViewModel>
            {
                PrintViewModels = new List<BudgetTargetPrintViewModel>() { viewModel },
                PaperWidth = PrintPaperWidth,
                PaperHeight = PrintPaperHeight
            };
            return View(printViewModel);
        }


        public ActionResult BatchPrint()
        {
            var idsString = Request.Params["targetIds"];
            if (string.IsNullOrWhiteSpace(idsString))
                return View(new PrintViewModel<BudgetTargetPrintViewModel>());

            var ids = JsonConvert.DeserializeObject<List<long>>(idsString);
            var targets = BudgetTargetService.GetTargets(ids);
            var viewModels = new List<BudgetTargetPrintViewModel>();
            foreach (var target in targets)
            {
                var parentFunctionSubjects = BasicDataService.GetParents(target.FunctionSubject);
                parentFunctionSubjects.Sort();
                var viewModel = target.CloneAnyMembers<BudgetTarget, BudgetTargetPrintViewModel>();
                viewModel.FunctionSubjects = parentFunctionSubjects;

                //锁定指标，不能再编辑
                if (!target.IsLocked)
                    BudgetTargetService.Lock(target.Id);

                viewModels.Add(viewModel);
            }

            var printViewModel = new PrintViewModel<BudgetTargetPrintViewModel>
            {
                PrintViewModels = viewModels,
                PaperWidth = PrintPaperWidth,
                PaperHeight = PrintPaperHeight
            };
            return View(printViewModel);
        }


        public JsonResult Unlock(long id = -1)
        {
            BudgetTargetService.Unlock(id);
            return Json(new BaseResponseDto { Message = "解除锁定成功！", Success = true, Title = _modelDescription });
        }

    }

}