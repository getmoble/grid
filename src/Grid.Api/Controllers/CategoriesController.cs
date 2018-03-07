using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Grid.Api.Controllers
{
    public class CategoriesController : GridApiBaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(ICategoryRepository categoryRepository,
                                  IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ContentResult Index()
        {
            var apiResult = TryExecute(() => _categoryRepository.GetAll(), "Categories Fetched sucessfully");

            var list = JsonConvert.SerializeObject(apiResult,
            Formatting.Indented,
            new JsonSerializerSettings()
             {
                 ReferenceLoopHandling = ReferenceLoopHandling.Ignore
             });

            return Content(list, "application/json");
        }
        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _categoryRepository.Get(id), "Categories fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Category category)
        {
            ApiResult<Category> apiResult;

            if (ModelState.IsValid)
            {
                if (category.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        category.UpdatedByUserId = WebUser.Id;
                        category.CreatedByUserId = WebUser.Id;
                        _categoryRepository.Update(category);
                        _unitOfWork.Commit();
                        return category;
                    }, "Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        category.CreatedByUserId = WebUser.Id;
                        _categoryRepository.Create(category);
                        _unitOfWork.Commit();

                        return category;
                    }, "Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Category>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

    }
}
