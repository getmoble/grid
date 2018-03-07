using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Entities;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.RMS.Controllers
{
    [GridPermission(PermissionCode = 225)]
    public class RequirementCategoriesController : GridBaseController
    {
        private readonly IRequirementCategoryRepository _requirementCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RequirementCategoriesController(IRequirementCategoryRepository requirementCategoryRepository,
                                               IUnitOfWork unitOfWork)
        {
            _requirementCategoryRepository = requirementCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var requirementCategories = _requirementCategoryRepository.GetAll();
            return View(requirementCategories);
        }

        public ActionResult Details(int id)
        {
            var requirementCategory = _requirementCategoryRepository.Get(id);
            return CheckForNullAndExecute(requirementCategory, () => View(requirementCategory));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequirementCategory requirementCategory)
        {
            if (ModelState.IsValid)
            {
                requirementCategory.CreatedByUserId = WebUser.Id;

                _requirementCategoryRepository.Create(requirementCategory);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(requirementCategory);
        }

        public ActionResult Edit(int id)
        {
            var requirementCategory = _requirementCategoryRepository.Get(id);
            return CheckForNullAndExecute(requirementCategory, () => View(requirementCategory));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequirementCategory requirementCategory)
        {
            if (ModelState.IsValid)
            {
                var selectedRequirementCategory = _requirementCategoryRepository.Get(requirementCategory.Id);

                if (selectedRequirementCategory != null)
                {
                    selectedRequirementCategory.Title = requirementCategory.Title;
                    selectedRequirementCategory.UpdatedByUserId = WebUser.Id;

                    _requirementCategoryRepository.Update(selectedRequirementCategory);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }
            return View(requirementCategory);
        }

        public ActionResult Delete(int id)
        {
            var requirementCategory = _requirementCategoryRepository.Get(id);
            return CheckForNullAndExecute(requirementCategory, () => View(requirementCategory));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _requirementCategoryRepository.Delete(id);
            _unitOfWork.Commit();

            return RedirectToAction("Index");
        }
    }
}
