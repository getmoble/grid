using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.IMS.Controllers
{
    [GridPermission(PermissionCode = 250)]
    public class AssetCategoriesController : InventoryBaseController
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssetCategoriesController(IAssetCategoryRepository assetCategoryRepository,
                                         IUnitOfWork unitOfWork)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var assetCategories = _assetCategoryRepository.GetAll();
            return View(assetCategories.ToList());
        }



    }
}
