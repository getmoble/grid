using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Areas.Recruit.Controllers
{
    public class CandidateDesignationsController : RecruitBaseController
    {
        private readonly ICandidateDesignationRepository _candidateDesignationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidateDesignationsController(ICandidateDesignationRepository candidateDesignationRepository,
                                               IUnitOfWork unitOfWork)
        {
            _candidateDesignationRepository = candidateDesignationRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var candidates = _candidateDesignationRepository.GetAll();
            return View(candidates);
        }

        public ActionResult Details(int id)
        {
            var candidateDesignation = _candidateDesignationRepository.Get(id);
            return CheckForNullAndExecute(candidateDesignation, () => View(candidateDesignation));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CandidateDesignation candidateDesignation)
        {
            if (ModelState.IsValid)
            {
                _candidateDesignationRepository.Create(candidateDesignation);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(candidateDesignation);
        }

        public ActionResult Edit(int id)
        {
            var candidateDesignation = _candidateDesignationRepository.Get(id);
            return CheckForNullAndExecute(candidateDesignation, () => View(candidateDesignation));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CandidateDesignation candidateDesignation)
        {
            if (ModelState.IsValid)
            {
                _candidateDesignationRepository.Update(candidateDesignation);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(candidateDesignation);
        }

        public ActionResult Delete(int id)
        {
            var candidateDesignation = _candidateDesignationRepository.Get(id);
            return CheckForNullAndExecute(candidateDesignation, () => View(candidateDesignation));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _candidateDesignationRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
