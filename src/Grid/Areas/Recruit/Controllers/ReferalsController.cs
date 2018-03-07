using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Areas.Recruit.Controllers
{
    public class ReferalsController : RecruitBaseController
    {
        private readonly IReferalRepository _referalRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReferalsController(IReferalRepository referalRepository,
                                  IJobOpeningRepository jobOpeningRepository,
                                  IUnitOfWork unitOfWork)
        {
            _referalRepository = referalRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var referals = _referalRepository.GetAll("JobOpening");
            var a = referals.ToList();
            return View();
        }

        public ActionResult Details(int id)
        {
            var referal = _referalRepository.Get(id);
            return CheckForNullAndExecute(referal, () => View(referal));
        }

        public ActionResult Create()
        {
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Referal referal)
        {
            if (ModelState.IsValid)
            {
                _referalRepository.Create(referal);
                _unitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title", referal.JobOpeningId);
            return View(referal);
        }

        public ActionResult Edit(int id)
        {
            var referal = _referalRepository.Get(id);
            return CheckForNullAndExecute(referal, () => View(referal));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Referal referal)
        {
            if (ModelState.IsValid)
            {
                _referalRepository.Update(referal);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title", referal.JobOpeningId);

            return View(referal);
        }

        public ActionResult Delete(int id)
        {
            var referal = _referalRepository.Get(id);
            return CheckForNullAndExecute(referal, () => View(referal));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _referalRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
