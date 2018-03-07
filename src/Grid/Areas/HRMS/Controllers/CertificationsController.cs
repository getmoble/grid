using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class CertificationsController : GridBaseController
    {
        private readonly ICertificationRepository _certificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CertificationsController(ICertificationRepository certificationRepository,
                                        IUnitOfWork unitOfWork)
        {
            _certificationRepository = certificationRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var certifications = _certificationRepository.GetAll(s => s.OrderBy(c => c.Title)).ToList();
            return View(certifications);
        }

        public ActionResult Details(int id)
        {
            var certification = _certificationRepository.Get(id);
            return CheckForNullAndExecute(certification, () => View(certification));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Certification certification)
        {
            if (ModelState.IsValid)
            {
                _certificationRepository.Create(certification);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(certification);
        }

        public ActionResult Edit(int id)
        {
            var certification = _certificationRepository.Get(id);
            return CheckForNullAndExecute(certification, () => View(certification));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Certification certification)
        {
            if (ModelState.IsValid)
            {
               _certificationRepository.Update(certification);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(certification);
        }

        public ActionResult Delete(int id)
        {
            var certification = _certificationRepository.Get(id);
            return CheckForNullAndExecute(certification, () => View(certification));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _certificationRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
