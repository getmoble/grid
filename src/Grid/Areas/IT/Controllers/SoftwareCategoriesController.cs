using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.IT.Controllers
{
    [GridPermission(PermissionCode = 230)]
    public class SoftwareCategoriesController : GridBaseController
    {
        private readonly ISoftwareCategoryRepository _softwareCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SoftwareCategoriesController(ISoftwareCategoryRepository softwareCategoryRepository,
                                  IUnitOfWork unitOfWork)
        {
            _softwareCategoryRepository = softwareCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var softwareCategories = _softwareCategoryRepository.GetAll();
            return View(softwareCategories);
        }
  
    }
}
