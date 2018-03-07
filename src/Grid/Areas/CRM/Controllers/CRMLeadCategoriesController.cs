using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMLeadCategoriesController : CRMBaseController
    {
        private readonly ICRMLeadCategoryRepository _crmLeadCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMLeadCategoriesController(ICRMLeadCategoryRepository crmLeadCategoryRepository,
                                       IUnitOfWork unitOfWork)
        {
            _crmLeadCategoryRepository = crmLeadCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var cRMLeadCategories = _crmLeadCategoryRepository.GetAllBy(o => o.OrderByDescending(c => c.CreatedOn), "CreatedByUser,UpdatedByUser");
            return View(cRMLeadCategories);
        }

        public ActionResult Details(int id)
        {
            var crmLeadStatus = _crmLeadCategoryRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRMLeadCategory cRMLeadCategory)
        {
            if (ModelState.IsValid)
            {
                cRMLeadCategory.CreatedByUserId = WebUser.Id;

                _crmLeadCategoryRepository.Create(cRMLeadCategory);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMLeadCategory);
        }

        public ActionResult Edit(int id)
        {
            var crmLeadStatus = _crmLeadCategoryRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CRMLeadCategory cRMLeadCategory)
        {
            if (ModelState.IsValid)
            {
                var selectedLeadCategory = _crmLeadCategoryRepository.Get(cRMLeadCategory.Id);

                if (selectedLeadCategory != null)
                {
                    selectedLeadCategory.Title = cRMLeadCategory.Title;
                    selectedLeadCategory.Description = cRMLeadCategory.Description;
                    selectedLeadCategory.UpdatedByUserId = WebUser.Id;

                    _crmLeadCategoryRepository.Update(selectedLeadCategory);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }

            return View(cRMLeadCategory);
        }

        public ActionResult Delete(int id)
        {
            var crmLeadStatus = _crmLeadCategoryRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _crmLeadCategoryRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
