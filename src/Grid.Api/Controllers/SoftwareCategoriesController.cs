using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;

namespace Grid.Api.Controllers
{
    public class SoftwareCategoriesController : GridApiBaseController
    {
        private readonly ISoftwareCategoryRepository _softwareCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SoftwareCategoriesController(ISoftwareCategoryRepository softwareCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _softwareCategoryRepository = softwareCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _softwareCategoryRepository.GetAll(), "Software Categories Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _softwareCategoryRepository.Get(id), "Software Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(SoftwareCategory softwareCategory)
        {
            ApiResult<SoftwareCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (softwareCategory.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _softwareCategoryRepository.Update(softwareCategory);
                        _unitOfWork.Commit();
                        return softwareCategory;
                    }, "Software Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _softwareCategoryRepository.Create(softwareCategory);
                        _unitOfWork.Commit();
                        return softwareCategory;
                    }, "Software Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<SoftwareCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _softwareCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Software Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
