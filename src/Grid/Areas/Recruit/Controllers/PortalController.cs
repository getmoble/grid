using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Grid.Areas.Recruit.ViewModels;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Features.Recruit.ViewModels;
using Grid.Features.Recruit.ViewModels.Candidate;
using Grid.Infrastructure;

namespace Grid.Areas.Recruit.Controllers
{
    public class PortalController : Controller
    {
        private const string PortalKey = "Portal_Candidate";
        private const string PortalKeyId = "Portal_Candidate_Id";
        private readonly ICandidateRepository _candidateRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IInterviewRoundRepository _interviewRoundRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly ICandidateTechnologyMapRepository _candidateTechnologyMapRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICandidateDesignationRepository _candidateDesignationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PortalController(ICandidateRepository candidateRepository,
                                IJobOpeningRepository jobOpeningRepository,
                                IInterviewRoundRepository interviewRoundRepository,
                                IRoundRepository roundRepository,
                                ICandidateTechnologyMapRepository candidateTechnologyMapRepository,
                                ITechnologyRepository technologyRepository,
                                ICandidateDesignationRepository candidateDesignationRepository,
                                IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _roundRepository = roundRepository;
            _interviewRoundRepository = interviewRoundRepository;
            _candidateTechnologyMapRepository = candidateTechnologyMapRepository;
            _technologyRepository = technologyRepository;
            _candidateDesignationRepository = candidateDesignationRepository;
            _unitOfWork = unitOfWork;
        }

        private ActionResult RedirectIfNotLoggedIn(Func<ActionResult> action)
        {
            var candidate = Session[PortalKey];
            var candidateId = Session[PortalKeyId];
            if (candidate != null && candidateId != null)
            {
                ViewBag.CandidateName = candidate;
                return action();
            }
            else
            {
                return RedirectToAction("SignIn", "Portal");
            }
        }

        public ActionResult SignIn()
        {
            var vm = new CandidateSignInViewModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult SignIn(CandidateSignInViewModel vm)
        {
            var selectedCandidate = _candidateRepository.GetBy(c => c.Email == vm.Email, "Person");
            if (selectedCandidate != null)
            {
                var hashedPassword = selectedCandidate.Password;
                var verificationSucceeded = hashedPassword != null && HashHelper.CheckHash(vm.Password, hashedPassword);

                if (verificationSucceeded)
                {
                    Session[PortalKeyId] = selectedCandidate.Id;
                    if (!string.IsNullOrEmpty(selectedCandidate.Person.Name))
                    {
                        Session[PortalKey] = selectedCandidate.Person.Name;
                    }
                    else
                    {
                        Session[PortalKey] = "Guest";
                    }

                    return RedirectToAction("Index", "Portal");
                }
            }
            return View();
        }

        public ActionResult Index()
        {
            return RedirectIfNotLoggedIn(View);
        }

        public ActionResult JobOpenings()
        {
            return RedirectIfNotLoggedIn(() =>
            {
                var jobOpenings = _jobOpeningRepository.GetAllBy(j => j.OpeningStatus == JobOpeningStatus.Open, j => j.OrderByDescending(d => d.CreatedOn)).ToList();
                return View(jobOpenings);
            });
        }

        public ActionResult OpeningDetails(int id)
        {
            return RedirectIfNotLoggedIn(() =>
            {
                var jobOpening = _jobOpeningRepository.Get(id);
                return View(jobOpening);
            });
        }

        public ActionResult Interviews(InterviewSearchViewModel vm)
        {
            return RedirectIfNotLoggedIn(() =>
            {
                ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
                ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title");
                var candidateId = int.Parse(Session[PortalKeyId].ToString());

                Func<IQueryable<InterviewRound>, IQueryable<InterviewRound>> interviewRoundFilter = q =>
                {
                    q = q.Include("Candidate.Person")
                            .Include(i => i.JobOpening)
                            .Include(i => i.Round);

                    q = q.Where(r => r.CandidateId == candidateId);

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
            });
        }

        public ActionResult EditProfile()
        {
            return RedirectIfNotLoggedIn(() =>
            {
                var candidateId = int.Parse(Session[PortalKeyId].ToString());
                var candidate = _candidateRepository.Get(candidateId, "Person");

                if (candidate == null)
                {
                    return HttpNotFound();
                }

                var mappedTechnologies = _candidateTechnologyMapRepository.GetAllBy(m => m.CandidateId == candidate.Id).Select(m => m.TechnologyId).ToList();

                ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
                ViewBag.CandidateDesignationId = new SelectList(_candidateDesignationRepository.GetAll(), "Id", "Title", candidate.DesignationId);
                var vm = new EditCandidateViewModel(candidate);
                return View(vm);
            });
        }

        [HttpPost]
        public ActionResult EditProfile(EditCandidateViewModel vm)
        {
            return RedirectIfNotLoggedIn(() =>
            {
                if (ModelState.IsValid)
                {
                    var selectedCandidate = _candidateRepository.Get(vm.Id, "Person");

                    if (selectedCandidate != null)
                    {
                        selectedCandidate.RecievedOn = vm.RecievedOn;
                        selectedCandidate.Source = vm.Source;
                        selectedCandidate.Qualification = vm.Qualification;
                        selectedCandidate.TotalExperience = vm.TotalExperience;
                        selectedCandidate.ResumePath = vm.ResumePath;
                        selectedCandidate.PhotoPath = vm.PhotoPath;
                        selectedCandidate.Status = vm.Status;
                        selectedCandidate.Comments = vm.Comments;
                        selectedCandidate.CurrentCTC = vm.CurrentCTC;
                        selectedCandidate.ExpectedCTC = vm.ExpectedCTC;
                        selectedCandidate.DesignationId = vm.CandidateDesignationId;

                        selectedCandidate.Person.FirstName = vm.Person.FirstName;
                        selectedCandidate.Person.LastName = vm.Person.LastName;
                        selectedCandidate.Person.Gender = vm.Person.Gender;
                        selectedCandidate.Person.Email = vm.Person.Email;
                        selectedCandidate.Person.Organization = vm.Person.Organization;
                        selectedCandidate.Person.Designation = vm.Person.Designation;
                        selectedCandidate.Person.PhoneNo = vm.Person.PhoneNo;
                        selectedCandidate.Person.SecondaryEmail = vm.Person.SecondaryEmail;
                        selectedCandidate.Person.OfficePhone = vm.Person.OfficePhone;
                        selectedCandidate.Person.Skype = vm.Person.Skype;
                        selectedCandidate.Person.Facebook = vm.Person.Facebook;
                        selectedCandidate.Person.Twitter = vm.Person.Twitter;
                        selectedCandidate.Person.GooglePlus = vm.Person.GooglePlus;
                        selectedCandidate.Person.LinkedIn = vm.Person.LinkedIn;
                        selectedCandidate.Person.Address = vm.Person.Address;
                        selectedCandidate.Person.CommunicationAddress = vm.Person.CommunicationAddress;
                        selectedCandidate.Person.DateOfBirth = vm.Person.DateOfBirth;

                        // Clean up white spaces for Email and Phone Numbers
                        if (!string.IsNullOrEmpty(selectedCandidate.Person.Email))
                        {
                            selectedCandidate.Person.Email = selectedCandidate.Person.Email.Trim();
                        }

                        if (!string.IsNullOrEmpty(selectedCandidate.Person.SecondaryEmail))
                        {
                            selectedCandidate.Person.SecondaryEmail = selectedCandidate.Person.SecondaryEmail.Trim();
                        }

                        if (!string.IsNullOrEmpty(selectedCandidate.Person.PhoneNo))
                        {
                            selectedCandidate.Person.PhoneNo = selectedCandidate.Person.PhoneNo.Trim();
                        }

                        if (!string.IsNullOrEmpty(selectedCandidate.Person.OfficePhone))
                        {
                            selectedCandidate.Person.OfficePhone = selectedCandidate.Person.OfficePhone.Trim();
                        }

                        // Update the Candidate Code, Fix for the existing missed candidates;
                        selectedCandidate.Code = $"LA{selectedCandidate.Id.ToString("D" + 6)}";

                        _candidateRepository.Update(selectedCandidate);
                        _unitOfWork.Commit();

                        // Remove the existing mapped Technologies 
                        var existingMaps = _candidateTechnologyMapRepository.GetAllBy(m => m.CandidateId == selectedCandidate.Id).ToList();
                        foreach (var map in existingMaps)
                        {
                            _candidateTechnologyMapRepository.Delete(map);
                        }

                        _unitOfWork.Commit();

                        // Map the New Technologies
                        if (vm.TechnologyIds != null)
                        {
                            foreach (var technologyId in vm.TechnologyIds)
                            {
                                var newMap = new CandidateTechnologyMap
                                {
                                    CandidateId = vm.Id,
                                    TechnologyId = technologyId
                                };

                                _candidateTechnologyMapRepository.Create(newMap);
                            }

                            _unitOfWork.Commit();
                        }

                        return RedirectToAction("Index");
                    }

                    return RedirectToAction("Index");
                }

                return View(vm);
            });
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            Session.Remove(PortalKey);
            Session.Remove(PortalKeyId);
            return RedirectToAction("SignIn", "Portal");
        }
    }
}