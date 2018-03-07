using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMPotentialCategoriesController : CRMBaseController
    {
        private readonly ICRMPotentialCategoryRepository _crmPotentialCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMPotentialCategoriesController(ICRMPotentialCategoryRepository crmPotentialCategoryRepository,
                                                IUnitOfWork unitOfWork)
        {
            _crmPotentialCategoryRepository = crmPotentialCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var crmPotentialCategories = _crmPotentialCategoryRepository.GetAll("CreatedByUser,UpdatedByUser");
            return View(crmPotentialCategories.OrderByDescending(c => c.CreatedOn).ToList());
        }

        public ActionResult Details(int id)
        {
            var crmPotentialCategory = _crmPotentialCategoryRepository.Get(id);
            return CheckForNullAndExecute(crmPotentialCategory, () => View(crmPotentialCategory));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRMPotentialCategory cRMPotentialCategory)
        {
            if (ModelState.IsValid)
            {
                cRMPotentialCategory.CreatedByUserId = WebUser.Id;

                _crmPotentialCategoryRepository.Create(cRMPotentialCategory);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMPotentialCategory);
        }

        public ActionResult Edit(int id)
        {
            var crmPotentialCategory = _crmPotentialCategoryRepository.Get(id);
            return CheckForNullAndExecute(crmPotentialCategory, () => View(crmPotentialCategory));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CRMPotentialCategory cRMPotentialCategory)
        {
            var selectedPotentialCategory = _crmPotentialCategoryRepository.Get(cRMPotentialCategory.Id);

            if (selectedPotentialCategory != null)
            {
                selectedPotentialCategory.Title = cRMPotentialCategory.Title;
                selectedPotentialCategory.Description = cRMPotentialCategory.Description;
                selectedPotentialCategory.UpdatedByUserId = WebUser.Id;

                _crmPotentialCategoryRepository.Update(selectedPotentialCategory);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMPotentialCategory);
        }

        public ActionResult Delete(int id)
        {
            var crmPotentialCategory = _crmPotentialCategoryRepository.Get(id);
            return CheckForNullAndExecute(crmPotentialCategory, () => View(crmPotentialCategory));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _crmPotentialCategoryRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
