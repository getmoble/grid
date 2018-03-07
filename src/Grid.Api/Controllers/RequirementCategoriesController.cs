using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;

namespace Grid.Api.Controllers
{
    public class RequirementCategoriesController : GridApiBaseController
    {
        private readonly IRequirementCategoryRepository _requirementCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequirementCategoriesController(IRequirementCategoryRepository requirementCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _requirementCategoryRepository = requirementCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _requirementCategoryRepository.GetAll(), "Requirement Categories Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _requirementCategoryRepository.Get(id), "Requirement Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(RequirementCategory vm)
        {
            ApiResult<RequirementCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var category = _requirementCategoryRepository.Get(vm.Id);
                        category.Title = vm.Title;
                        category.UpdatedByUserId = WebUser.Id;
                        category.UpdatedOn = DateTime.UtcNow;
                        _requirementCategoryRepository.Update(category);
                        _unitOfWork.Commit();
                        return category;
                    }, "Requirement Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        vm.CreatedByUserId = WebUser.Id;
                        _requirementCategoryRepository.Create(vm);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Requirement Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<RequirementCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _requirementCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Requirement Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
