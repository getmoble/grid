using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.EmailService;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Models;
using Newtonsoft.Json;
using Grid.Providers.Blob;
using Grid.Features.HRMS.Entities.Enums;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Features.Recruit.Services.Interfaces;
using Grid.Features.Recruit.ViewModels;
using Grid.Features.Recruit.ViewModels.Candidate;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Filters;

namespace Grid.Areas.Recruit.Controllers
{
    public class CandidatesController : RecruitBaseController
    {
        private readonly ICandidateDesignationRepository _candidateDesignationRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICandidateActivityRepository _candidateActivityRepository;
        private readonly IInterviewRoundRepository _interviewRoundRepository;
        private readonly IInterviewRoundActivityRepository _interviewRoundActivityRepository;
        private readonly ICandidateTechnologyMapRepository _candidateTechnologyMapRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IRoundRepository _roundRepository;
        private readonly ICandidateDocumentRepository _candidateDocumentRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICandidateService _candidateService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISettingsService _settingsService;
        private readonly EmailComposerService _emailComposerService;

        public CandidatesController(ICandidateDesignationRepository candidateDesignationRepository,
                                    ICandidateRepository candidateRepository,
                                    ICandidateActivityRepository candidateActivityRepository,
                                    IInterviewRoundRepository interviewRoundRepository,
                                    IInterviewRoundActivityRepository interviewRoundActivityRepository,
                                    ICandidateTechnologyMapRepository candidateTechnologyMapRepository,
                                    IUserRepository userRepository,
                                    IJobOpeningRepository jobOpeningRepository,
                                    IRoundRepository roundRepository,
                                    ICandidateDocumentRepository candidateDocumentRepository,
                                    ITechnologyRepository technologyRepository,
                                    ICandidateService candidateService,
                                    ISettingsService settingsService,
                                    EmailComposerService emailComposerService,
                                    IUnitOfWork unitOfWork)
        {
            _candidateDesignationRepository = candidateDesignationRepository;
            _candidateRepository = candidateRepository;
            _candidateActivityRepository = candidateActivityRepository;
            _interviewRoundRepository = interviewRoundRepository;
            _interviewRoundActivityRepository = interviewRoundActivityRepository;
            _userRepository = userRepository;
            _jobOpeningRepository = jobOpeningRepository;
            _roundRepository = roundRepository;
            _candidateDocumentRepository = candidateDocumentRepository;
            _candidateService = candidateService;
            _candidateTechnologyMapRepository = candidateTechnologyMapRepository;
            _technologyRepository = technologyRepository;
            _emailComposerService = emailComposerService;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
        }

        #region Ajax Calls
        [HttpGet]
        public string GetAllActivitiesForCandidate(int id)
        {
            var interviews = _interviewRoundRepository.GetAllBy(i => i.CandidateId == id).Select(i => i.Id);
            var interviewActivities = _interviewRoundActivityRepository.GetAllBy(a => interviews.Contains(a.InterviewRoundId)).Select(a => new ActivityModel()
            {
                Title = a.Title,
                Comment = a.Comment,
                CreatedOn = a.CreatedOn
            }).ToList();

            var activities = _candidateActivityRepository.GetAllBy(r => r.CandidateId == id, o => o.OrderByDescending(r => r.CreatedOn)).Select(a => new ActivityModel()
            {
                Title = a.Title,
                Comment = a.Comment,
                CreatedOn = a.CreatedOn
            }).ToList();

            interviewActivities.AddRange(activities);
            interviewActivities = interviewActivities.OrderByDescending(o => o.CreatedOn).ToList();
            var list = JsonConvert.SerializeObject(interviewActivities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }

        private string GetCandidateDetails(Candidate candidate)
        {
            var builder = new StringBuilder();
            builder.Append("<p>Already a Candidate exists with the same details</p>");
            builder.Append($"<p>Name is: {candidate.Person.Name}</p>");
            builder.Append($"<p>Email is: {candidate.Person.Email}</p>");
            builder.Append($"<p>Secondary Email is: {candidate.Person.SecondaryEmail}</p>");
            builder.Append($"<p>Phone No is: {candidate.Person.PhoneNo}</p>");
            builder.Append($"<p>Office Phone No is: {candidate.Person.OfficePhone}</p>");
            builder.Append($"<a target='_blank' href='/Recruit/Candidates/Details/{candidate.Id}'>View Details</a>");
            return builder.ToString();
        }

        [HttpPost]
        public JsonResult ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var selectedCandidate = _candidateRepository.GetBy(c => c.Person.Email == email || c.Person.SecondaryEmail == email, "Person");
                if (selectedCandidate != null)
                {
                    return Json(new RemoteValidationResult
                    {
                        Status = false,
                        Message = GetCandidateDetails(selectedCandidate)
                    });
                }

                return Json(new RemoteValidationResult
                {
                    Status = true,
                    Message = "No Candidate Found"
                });
            }

            return Json(new RemoteValidationResult
            {
                Status = true,
                Message = "No Candidate Found"
            });
        }

        [HttpPost]
        public JsonResult ValidatePhoneNo(string phoneNo)
        {
            if (!string.IsNullOrEmpty(phoneNo))
            {
                var selectedCandidate = _candidateRepository.GetBy(c => c.Person.PhoneNo == phoneNo || c.Person.OfficePhone == phoneNo, "Person");
                if (selectedCandidate != null)
                {
                    return Json(new RemoteValidationResult
                    {
                        Status = false,
                        Message = GetCandidateDetails(selectedCandidate)
                    });
                }

                return Json(new RemoteValidationResult
                {
                    Status = true,
                    Message = "No Candidate Found"
                });
            }

            return Json(new RemoteValidationResult
            {
                Status = true,
                Message = "No Candidate Found"
            });
        }

        [HttpPost]
        public JsonResult AddNote(CandidateActivityViewModel vm)
        {
            var selectedCandidate = _candidateRepository.Get(vm.CandidateId);
            if (selectedCandidate != null)
            {
                // Add it as an Activity
                var newActivity = new CandidateActivity
                {
                    Title = vm.Title,
                    Comment = vm.Comment,
                    CandidateId = selectedCandidate.Id,
                    CreatedByUserId = WebUser.Id
                };

                _candidateActivityRepository.Create(newActivity);
                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult DeleteNote(int id)
        {
            _candidateActivityRepository.Delete(id);
            _unitOfWork.Commit();

            return Json(true);
        }

        [HttpPost]
        public JsonResult AddInterviewRound(CreateInterviewRoundViewModel vm)
        {
            try
            {
                var interviews = new List<int>();
                foreach (var interviewer in vm.InterviewerIds.ToList())
                {
                    var newInterviewRound = new InterviewRound
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

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        #endregion

        public ActionResult Index(CandidateSearchViewModel vm)
        {
            var candidateDesignations = _candidateDesignationRepository.GetAll();
            ViewBag.CandidateDesignationId = new SelectList(candidateDesignations, "Id", "Title");

            Func<IQueryable<Candidate>, IQueryable<Candidate>> candidateFilter = q =>
            {
                q = q.Include(c => c.Person).Include(c => c.Designation);

                if (vm.CandidateDesignationId.HasValue)
                {
                    q = q.Where(r => r.DesignationId == vm.CandidateDesignationId.Value);
                }

                if (vm.MinExperience.HasValue)
                {
                    q = q.Where(r => r.TotalExperience >= vm.MinExperience.Value);
                }

                if (vm.MaxExperience.HasValue)
                {
                    q = q.Where(r => r.TotalExperience <= vm.MaxExperience.Value);
                }

                if (vm.Source.HasValue)
                {
                    q = q.Where(r => r.Source == vm.Source.Value);
                }

                if (vm.Status.HasValue)
                {
                    q = q.Where(r => r.Status == vm.Status.Value);
                }

                if (!string.IsNullOrEmpty(vm.FirstName))
                {
                    q = q.Where(r => r.Person.FirstName.Contains(vm.FirstName));
                }

                if (!string.IsNullOrEmpty(vm.LastName))
                {
                    q = q.Where(r => r.Person.LastName.Contains(vm.LastName));
                }

                if (vm.Gender.HasValue)
                {
                    q = q.Where(r => r.Person.Gender == vm.Gender.Value);
                }

                if (!string.IsNullOrEmpty(vm.Organization))
                {
                    q = q.Where(r => r.Person.Organization.Contains(vm.Organization));
                }

                if (!string.IsNullOrEmpty(vm.Designation))
                {
                    q = q.Where(r => r.Person.Designation.Contains(vm.Designation));
                }

                if (!string.IsNullOrEmpty(vm.Email))
                {
                    q = q.Where(r => r.Person.Email.Contains(vm.Email));
                }

                if (!string.IsNullOrEmpty(vm.Phone))
                {
                    q = q.Where(r => r.Person.PhoneNo.Contains(vm.Phone));
                }

                return q;
            };

            vm.Candidates = _candidateRepository.SearchPage(candidateFilter, o => o.OrderByDescending(c => c.RecievedOn), vm.GetPageNo(), vm.PageSize);

            return View(vm);
        }

        public ActionResult Details(int id)
        {
            var candidate = _candidateRepository.Get(id, "Person");

            if (candidate == null)
            {
                return HttpNotFound();
            }

            ViewBag.InterviewerId = new MultiSelectList(_userRepository.GetAllBy(u => u.EmployeeStatus != EmployeeStatus.Ex && u.Id != 1, "Person"), "Id", "Person.Name");
            ViewBag.JobOpeningId = new SelectList(_jobOpeningRepository.GetAll(), "Id", "Title");
            ViewBag.RoundId = new SelectList(_roundRepository.GetAll(), "Id", "Title");

            var candidateDocs = _candidateDocumentRepository.GetAllBy(m => m.CandidateId == candidate.Id);
            var rounds = _interviewRoundRepository.GetAllBy(m => m.CandidateId == candidate.Id, "Interviewer.Person,Round");

            var vm = new CandidateDetailsViewModel(candidate)
            {
                CandidateDocuments = candidateDocs.ToList(),
                InterviewRounds = rounds.ToList()
            };

            return View(vm);
        }

        [MultiSelectList("Technology", "Technologies")]
        public ActionResult Create()
        {
            ViewBag.CandidateDesignationId = new SelectList(_candidateDesignationRepository.GetAll(), "Id", "Title");

            return View(new NewCandidateViewModel
            {
                RecievedOn = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiSelectList("Technology", "Technologies", MultiSelectListState.Recreate)]
        public ActionResult Create(NewCandidateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newCandidate = new Candidate
                {
                    Source = vm.Source,
                    Qualification = vm.Qualification,
                    TotalExperience = vm.TotalExperience,
                    ResumePath = vm.ResumePath,
                    PhotoPath = vm.PhotoPath,
                    Status = vm.Status,
                    Comments = vm.Comments,
                    CurrentCTC = vm.CurrentCTC,
                    ExpectedCTC = vm.ExpectedCTC,
                    Person = vm.Person,
                    DesignationId = vm.CandidateDesignationId
                };


                // Clean up white spaces for Email and Phone Numbers
                if (!string.IsNullOrEmpty(newCandidate.Person.Email))
                {
                    newCandidate.Person.Email = newCandidate.Person.Email.Trim();
                }
                
                if (!string.IsNullOrEmpty(newCandidate.Person.SecondaryEmail))
                {
                    newCandidate.Person.SecondaryEmail = newCandidate.Person.SecondaryEmail.Trim();
                }

                if (!string.IsNullOrEmpty(newCandidate.Person.PhoneNo))
                {
                    newCandidate.Person.PhoneNo = newCandidate.Person.PhoneNo.Trim();
                }

                if (!string.IsNullOrEmpty(newCandidate.Person.OfficePhone))
                {
                    newCandidate.Person.OfficePhone = newCandidate.Person.OfficePhone.Trim();
                }

                newCandidate.RecievedOn = vm.RecievedOn;

                newCandidate.CreatedByUserId = WebUser.Id;

                _candidateRepository.Create(newCandidate);
                _unitOfWork.Commit();

                // Update the Candidate Code.
                var selectedCandidate = _candidateRepository.Get(newCandidate.Id);
                if (selectedCandidate != null)
                {
                    selectedCandidate.Code = $"LA{selectedCandidate.Id.ToString("D" + 6)}";
                    _candidateRepository.Update(selectedCandidate);
                    _unitOfWork.Commit();
                }

                // Map the Technologies
                if (vm.TechnologyIds != null)
                {
                    foreach (var technologyId in vm.TechnologyIds)
                    {
                        var newMap = new CandidateTechnologyMap
                        {
                            CandidateId = newCandidate.Id,
                            TechnologyId = technologyId
                        };

                        _candidateTechnologyMapRepository.Create(newMap);
                    }

                    _unitOfWork.Commit();
                }

                // Add the resume and map it to docs.
                if (vm.Resume != null)
                {
                    var newDocument = new CandidateDocument
                    {
                        CandidateId = newCandidate.Id,
                        DocumentType = CandidateDocumentType.Resume
                    };

                    var siteSettings = _settingsService.GetSiteSettings();
                    var blobUploadService = new BlobUploadService(siteSettings.BlobSettings);
                    var blobPath = blobUploadService.UploadRecruitDocument(vm.Resume);
                    newDocument.DocumentPath = blobPath;
                    newDocument.FileName = vm.Resume.FileName;

                    _candidateDocumentRepository.Create(newDocument);
                    _unitOfWork.Commit();
                }

                return RedirectToAction("Index");
            }

            ViewBag.CandidateDesignationId = new SelectList(_candidateDesignationRepository.GetAll(), "Id", "Title");
            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var candidate = _candidateRepository.Get(id, "Person");

            if (candidate == null)
            {
                return HttpNotFound();
            }

            var mappedTechnologies = _candidateTechnologyMapRepository.GetAllBy(m => m.CandidateId == candidate.Id).Select(m => m.TechnologyId).ToList();

            ViewBag.Technologies = new MultiSelectList(_technologyRepository.GetAll(), "Id", "Title", mappedTechnologies);
            ViewBag.CandidateDesignationId = new SelectList(_candidateDesignationRepository.GetAll(), "Id", "Title", candidate.DesignationId);
            var vm = new EditCandidateViewModel(candidate);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCandidateViewModel vm)
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

                    selectedCandidate.UpdatedByUserId = WebUser.Id;


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
        }

        public ActionResult Delete(int id)
        {
            var candidate = _candidateRepository.Get(id);
            return CheckForNullAndExecute(candidate, () => View(candidate));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var operationResult = _candidateService.Delete(id);
            if (operationResult.Status)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, operationResult.Message);
            return View();
        }
    }
}
