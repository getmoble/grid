using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Newtonsoft.Json;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Features.Recruit.ViewModels;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.Recruit.Controllers
{
    public class InterviewRoundsController : RecruitBaseController
    {
        private readonly IInterviewRoundRepository _interviewRoundRepository;
        private readonly IInterviewRoundActivityRepository _interviewRoundActivityRepository;
        private readonly IInterviewRoundDocumentRepository _interviewRoundDocumentRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly EmailComposerService _emailComposerService;

        public InterviewRoundsController(IInterviewRoundRepository interviewRoundRepository,
                                         IInterviewRoundActivityRepository interviewRoundActivityRepository,
                                         IInterviewRoundDocumentRepository interviewRoundDocumentRepository,
                                         ICandidateRepository candidateRepository,
                                         IRoundRepository roundRepository,
                                         IUserRepository userRepository,
                                         IJobOpeningRepository jobOpeningRepository,
                                         EmailComposerService emailComposerService,
                                         IUnitOfWork unitOfWork)
        {
            _interviewRoundRepository = interviewRoundRepository;
            _interviewRoundActivityRepository = interviewRoundActivityRepository;
            _interviewRoundDocumentRepository = interviewRoundDocumentRepository;
            _candidateRepository = candidateRepository;
            _userRepository = userRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _roundRepository = roundRepository;
            _unitOfWork = unitOfWork;

            _emailComposerService = emailComposerService;
        }

        #region Ajax Calls
        [HttpGet]
        public string GetAllActivitiesForInterviewRound(int id)
        {
            var activities = _interviewRoundActivityRepository.GetAllBy(r => r.InterviewRoundId == id, o => o.OrderByDescending(r => r.CreatedOn)).ToList();
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }

        [HttpPost]
        public JsonResult AddNote(InterviewRoundActivityViewModel vm)
        {
            var selectedInterviewRound = _interviewRoundRepository.Get(vm.InterviewRoundId);
            if (selectedInterviewRound != null)
            {
                // Add it as an Activity
                var newActivity = new InterviewRoundActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    InterviewRoundId = selectedInterviewRound.Id,
                    CreatedByUserId = WebUser.Id
                };

                _interviewRoundActivityRepository.Create(newActivity);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteNote(int id)
        {
            _interviewRoundActivityRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(true);
        }

        #endregion

        public ActionResult Index(InterviewSearchViewModel vm)
        {
            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name");
            ViewBag.InterviewerId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name");
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title");

            Func<IQueryable<InterviewRound>, IQueryable<InterviewRound>> interviewRoundFilter = q =>
            {
                q = q.Include("Candidate.Person")
                        .Include("Interviewer.Person")
                        .Include(i => i.JobOpening)
                        .Include(i => i.Round);


                if (vm.JobId.HasValue)
                {
                    q = q.Where(r => r.JobOpeningId == vm.JobId.Value);
                }

                if (vm.CandidateId.HasValue)
                {
                    q = q.Where(r => r.CandidateId == vm.CandidateId.Value);
                }

                if (vm.InterviewerId.HasValue)
                {
                    q = q.Where(r => r.InterviewerId == vm.InterviewerId.Value);
                }

                if (vm.RoundId.HasValue)
                {
                    q = q.Where(r => r.RoundId == vm.RoundId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }

                return q;
            };

            vm.Interviews = _interviewRoundRepository.SearchPage(interviewRoundFilter, o => o.OrderByDescending(c => c.ScheduledOn), vm.GetPageNo(), vm.PageSize);

            return View(vm);
        }

        [GridPermission(PermissionCode = 300)]
        public ActionResult MyInterviews(InterviewSearchViewModel vm)
        {
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title");

            Func<IQueryable<InterviewRound>, IQueryable<InterviewRound>> interviewRoundFilter = q =>
            {
                q = q.Include("Candidate.Person")
                        .Include(i => i.JobOpening)
                        .Include(i => i.Round);

                q = q.Where(r => r.InterviewerId == WebUser.Id);

                if (vm.JobId.HasValue)
                {
                    q = q.Where(r => r.JobOpeningId == vm.JobId.Value);
                }

                if (vm.RoundId.HasValue)
                {
                    q = q.Where(r => r.RoundId == vm.RoundId.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }

                return q;
            };

            vm.Interviews = _interviewRoundRepository.SearchPage(interviewRoundFilter, o => o.OrderByDescending(c => c.ScheduledOn), vm.GetPageNo(), vm.PageSize);
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var interviewRound = _interviewRoundRepository.Get(id, "JobOpening,Candidate.Person,Round");

            if (interviewRound == null)
            {
                return HttpNotFound();
            }

            var interviewDocs = _interviewRoundDocumentRepository.GetAllBy(m => m.InterviewRoundId == interviewRound.Id);
            var activities = _interviewRoundActivityRepository.GetAllBy(m => m.InterviewRoundId == interviewRound.Id);

            var vm = new InterviewRoundDetailsViewModel(interviewRound)
            {
                InterviewRoundDocuments = interviewDocs.ToList(),
                InterviewRoundActivities = activities.ToList()
            };

            return View(vm);
        }

        public ActionResult Create()
        {
            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name");
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title");
            ViewBag.InterviewerId = new MultiSelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name");

            var vm = new CreateInterviewRoundViewModel();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateInterviewRoundViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var interviews = new List<int>();
                foreach (var interviewer in vm.InterviewerIds.ToList())
                {
                    var newInterviewRound = new InterviewRound()
                    {
                        JobOpeningId = vm.JobOpeningId,
                        CandidateId = vm.CandidateId,
                        RoundId = vm.RoundId,
                        InterviewerId = interviewer,
                        ScheduledOn = vm.ScheduledOn,
                        Status = InterviewStatus.Scheduled,
                        Comments = vm.Comments
                    };

                    _interviewRoundRepository.Create(newInterviewRound);
                    _unitOfWork.Commit();
                    interviews.Add(newInterviewRound.Id);
                }

                foreach (var interviewId in interviews)
                {
                    #if !DEBUG
                        _emailComposerService.InterviewScheduled(interviewId);
                    #endif
                }

                return RedirectToAction("Index");
            }

            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name");
            ViewBag.InterviewerId = new MultiSelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", vm.InterviewerIds);
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title");

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var interviewRound = _interviewRoundRepository.Get(id);

            if (interviewRound == null)
            {
                return HttpNotFound();
            }

            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name", interviewRound.CandidateId);
            ViewBag.InterviewerId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", interviewRound.InterviewerId);
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title", interviewRound.JobOpeningId);
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title", interviewRound.RoundId);

            return View(interviewRound);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InterviewRound interviewRound)
        {
            if (ModelState.IsValid)
            {
                _interviewRoundRepository.Update(interviewRound);
                _unitOfWork.Commit();

                return RedirectToAction("Index");
            }

            ViewBag.CandidateId = new SelectList(_candidateRepository.GetAll(o => o.OrderByDescending(c => c.Id), "Person").Select(c => new { c.Id, Name = c.Person.Name + "- [" + c.Code + "]" }), "Id", "Name", interviewRound.CandidateId);
            ViewBag.InterviewerId = new SelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name", interviewRound.InterviewerId);
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title", interviewRound.JobOpeningId);
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title", interviewRound.RoundId);

            return View(interviewRound);
        }

        public ActionResult ChangeStatus(int id)
        {
            var interviewRound = _interviewRoundRepository.Get(id);

            if (interviewRound == null)
            {
                return HttpNotFound();
            }

            // Reset comments, as we need to use it for activity.
            interviewRound.Comments = string.Empty;

            return View(interviewRound);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(InterviewRound vm)
        {
            if (ModelState.IsValid)
            {
                var selectedInterviewRound = _interviewRoundRepository.Get(vm.Id);
                if (selectedInterviewRound != null)
                {
                    selectedInterviewRound.Status = vm.Status;

                    var newActivity = new InterviewRoundActivity
                    {
                        InterviewRoundId = selectedInterviewRound.Id,
                        Title = "Status Changed",
                        Comment = vm.Comments,
                        CreatedByUserId = WebUser.Id
                    };

                    _interviewRoundActivityRepository.Create(newActivity);
                    _unitOfWork.Commit();
                }

                _interviewRoundRepository.Update(selectedInterviewRound);
                _unitOfWork.Commit();

                return RedirectToAction("Details", "InterviewRounds", new { id = vm.Id });
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var interviewRound = _interviewRoundRepository.Get(id);

            if (interviewRound == null)
            {
                return HttpNotFound();
            }

            return View(interviewRound);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var interviewRound = _interviewRoundRepository.Get(id);
            _interviewRoundRepository.Delete(interviewRound);
            _unitOfWork.Commit();
            return RedirectToAction("Details", "Candidates", new { id = interviewRound.CandidateId });
        }
    }
}
