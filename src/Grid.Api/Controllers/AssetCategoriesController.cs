using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;

namespace Grid.Api.Controllers
{
    public class AssetCategoriesController : GridApiBaseController
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssetCategoriesController(IAssetCategoryRepository assetCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() => _assetCategoryRepository.GetAll(), "Asset Categories Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _assetCategoryRepository.Get(id), "Asset Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(AssetCategory holiday)
        {
            ApiResult<AssetCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (holiday.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _assetCategoryRepository.Update(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Asset Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _assetCategoryRepository.Create(holiday);
                        _unitOfWork.Commit();
                        return holiday;
                    }, "Asset Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<AssetCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _assetCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Asset Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
