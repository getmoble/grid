using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;

namespace Grid.Api.Controllers
{
    public class ArticleCategoriesController : GridApiBaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleCategoriesController(ICategoryRepository categoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _categoryRepository.GetAll(), "Categories Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _categoryRepository.Get(id), "Category fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Category vm)
        {
            ApiResult<Category> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var category = _categoryRepository.Get(vm.Id);
                        category.Title = vm.Title;
                        category.Description = vm.Description;
                        category.IsPublic = vm.IsPublic;
                        category.ParentCategoryId = vm.ParentCategoryId;
                        category.UpdatedByUserId = WebUser.Id;
                        category.UpdatedOn = DateTime.UtcNow;

                        _categoryRepository.Update(category);
                        _unitOfWork.Commit();
                        return category;
                    }, "Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        vm.CreatedByUserId = WebUser.Id;
                        _categoryRepository.Create(vm);
                        _unitOfWork.Commit();
                        return vm;
                    }, "Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Category>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _categoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
