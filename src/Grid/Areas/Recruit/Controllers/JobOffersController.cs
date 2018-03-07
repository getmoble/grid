using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;

namespace Grid.Areas.Recruit.Controllers
{
    public class JobOffersController : RecruitBaseController
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IJobOfferRepository _jobOfferRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JobOffersController(IJobOfferRepository jobOfferRepository,
                                   ICandidateRepository candidateRepository,
                                   IDesignationRepository designationRepository,
                                   IUnitOfWork unitOfWork)
        {
            _jobOfferRepository = jobOfferRepository;
            _candidateRepository = candidateRepository;
            _designationRepository = designationRepository; 
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var jobOffers = _jobOfferRepository.GetAll("Candidate,Designation").ToList();
            return View(jobOffers);
        }

        public ActionResult Details(int id)
        {
            var jobOffer = _jobOfferRepository.Get(id);
            return CheckForNullAndExecute(jobOffer, () => View(jobOffer));
        }

        public ActionResult Create(int? candidateId)
        {
            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name", candidateId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAll(), "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobOffer jobOffer)
        {
            if (ModelState.IsValid)
            {
                var selectedCandidate = _candidateRepository.Get(jobOffer.CandidateId);
                if (selectedCandidate != null)
                {
                    selectedCandidate.Status = CandidateStatus.Offered;
                    _candidateRepository.Update(selectedCandidate);
                }

                _jobOfferRepository.Create(jobOffer);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name", jobOffer.CandidateId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAll(), "Id", "Title", jobOffer.DesignationId);

            return View(jobOffer);
        }

        public ActionResult Edit(int id)
        {
            var jobOffer = _jobOfferRepository.Get(id);

            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll("Person"), "Id", "Source", jobOffer.CandidateId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAll(), "Id", "Title", jobOffer.DesignationId);

            return CheckForNullAndExecute(jobOffer, () => View(jobOffer));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobOffer jobOffer)
        {
            if (ModelState.IsValid)
            {
                _jobOfferRepository.Update(jobOffer);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll("Person"), "Id", "Source", jobOffer.CandidateId);
            ViewBag.DesignationId = new SelectList(_designationRepository.GetAll(), "Id", "Title", jobOffer.DesignationId);

            return View(jobOffer);
        }

        public ActionResult Delete(int id)
        {
            var jobOffer = _jobOfferRepository.Get(id);
            return CheckForNullAndExecute(jobOffer, () => View(jobOffer));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _jobOfferRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
