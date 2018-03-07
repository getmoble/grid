using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.CRM.Controllers
{
    [GridPermission(PermissionCode = 220)]
    public class CRMLeadSourcesController : CRMBaseController
    {
        private readonly ICRMLeadSourceRepository _crmLeadSourceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMLeadSourcesController(ICRMLeadSourceRepository crmLeadSourceRepository,
                                        IUnitOfWork unitOfWork)
        {
            _crmLeadSourceRepository = crmLeadSourceRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var crmLeadSources = _crmLeadSourceRepository.GetAllBy(o => o.OrderByDescending(f => f.CreatedOn));
            return View(crmLeadSources);
        }

        public ActionResult Details(int id)
        {
            var crmLeadStatus = _crmLeadSourceRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CRMLeadSource cRMLeadSource)
        {
            if (ModelState.IsValid)
            {
                cRMLeadSource.CreatedByUserId = WebUser.Id;

                _crmLeadSourceRepository.Create(cRMLeadSource);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(cRMLeadSource);
        }

        public ActionResult Edit(int id)
        {
            var crmLeadStatus = _crmLeadSourceRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CRMLeadSource cRMLeadSource)
        {
            if (ModelState.IsValid)
            {
                var selectedLeadSource = _crmLeadSourceRepository.Get(cRMLeadSource.Id);

                if (selectedLeadSource != null)
                {
                    selectedLeadSource.Title = cRMLeadSource.Title;
                    selectedLeadSource.Description = cRMLeadSource.Description;
                    selectedLeadSource.UpdatedByUserId = WebUser.Id;

                    _crmLeadSourceRepository.Update(cRMLeadSource);
                    _unitOfWork.Commit();

                    return RedirectToAction("Index");
                }
            }
            return View(cRMLeadSource);
        }

        public ActionResult Delete(int id)
        {
            var crmLeadStatus = _crmLeadSourceRepository.Get(id);
            return CheckForNullAndExecute(crmLeadStatus, () => View(crmLeadStatus));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _crmLeadSourceRepository.Delete(id);
            _unitOfWork.Commit();

            return RedirectToAction("Index");
        }
    }
}
