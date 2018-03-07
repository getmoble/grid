using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Features.Recruit.ViewModels;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Recruit.Controllers
{
    public class JobOpeningsController : RecruitBaseController
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JobOpeningsController(IJobOpeningRepository jobOpeningRepository,
                                     IUnitOfWork unitOfWork)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _unitOfWork = unitOfWork;
        }

        [GridPermission(PermissionCode = 500)]
        public ActionResult Index()
        {
            var jobOpenings = _jobOpeningRepository.GetAll(j => j.OrderByDescending(d => d.CreatedOn)).ToList();
            return View(jobOpenings);
        }

        // Public - Exposed to all Employees
        [GridPermission(PermissionCode = 300)]
        public ActionResult Openings()
        {
            var jobOpenings = _jobOpeningRepository.GetAllBy(j => j.OpeningStatus == JobOpeningStatus.Open, j => j.OrderByDescending(d => d.CreatedOn)).ToList();
            return View(jobOpenings);
        }

        // Public - Exposed to all Employees
        [GridPermission(PermissionCode = 300)]
        public ActionResult OpeningDetails(int id)
        {
            var jobOpening = _jobOpeningRepository.Get(id);
            return CheckForNullAndExecute(jobOpening, () => View(jobOpening));
        }

        [GridPermission(PermissionCode = 500)]
        public ActionResult Details(int id)
        {
            var jobOpening = _jobOpeningRepository.Get(id);
            return CheckForNullAndExecute(jobOpening, () => View(jobOpening));
        }

        [GridPermission(PermissionCode = 500)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [GridPermission(PermissionCode = 500)]
        public ActionResult Create(JobOpeningViewModel jobOpening)
        {
            if (ModelState.IsValid)
            {
                var newJobOpening = new JobOpening
                {
                    Title = jobOpening.Title,
                    NoOfVacancies = jobOpening.NoOfVacancies,
                    Description = jobOpening.Description,
                    OpeningStatus = jobOpening.OpeningStatus,
                    JobDescriptionPath = jobOpening.JobDescriptionFilePath
                };

                _jobOpeningRepository.Create(newJobOpening);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            return View(jobOpening);
        }

        [GridPermission(PermissionCode = 500)]
        public ActionResult Edit(int id)
        {
            var jobOpening = _jobOpeningRepository.Get(id);

            var jobOpeningViewmodel = new JobOpeningViewModel
            {
                Id = jobOpening.Id,
                Title = jobOpening.Title,
                Description = jobOpening.Description,
                OpeningStatus = jobOpening.OpeningStatus,
                JobDescriptionFilePath = jobOpening.JobDescriptionPath,
                NoOfVacancies = jobOpening.NoOfVacancies
            };

            return View(jobOpeningViewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [GridPermission(PermissionCode = 500)]
        public ActionResult Edit(JobOpeningViewModel jobOpening)
        {
            if (ModelState.IsValid)
            {
                var selectedJobOpening = _jobOpeningRepository.Get(jobOpening.Id);

                selectedJobOpening.Title = jobOpening.Title;
                selectedJobOpening.NoOfVacancies = jobOpening.NoOfVacancies;
                selectedJobOpening.Description = jobOpening.Description;
                selectedJobOpening.OpeningStatus = jobOpening.OpeningStatus;
                selectedJobOpening.JobDescriptionPath = jobOpening.JobDescriptionFilePath;

                _jobOpeningRepository.Update(selectedJobOpening);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }
            return View(jobOpening);
        }

        [GridPermission(PermissionCode = 500)]
        public ActionResult Delete(int id)
        {
            var jobOpening = _jobOpeningRepository.Get(id);
            return CheckForNullAndExecute(jobOpening, () => View(jobOpening));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [GridPermission(PermissionCode = 500)]
        public ActionResult DeleteConfirmed(int id)
        {
            _jobOpeningRepository.Delete(id);
            _unitOfWork.Commit();
            return RedirectToAction("Index");
        }
    }
}
