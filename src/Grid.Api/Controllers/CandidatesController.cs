using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Api.Models.Recruit;
using Grid.Features.Common;
using Newtonsoft.Json;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.ViewModels.Candidate;

namespace Grid.Api.Controllers
{
    public class CandidatesController: GridApiBaseController
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ICandidateActivityRepository _candidateActivityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidatesController(ICandidateRepository candidateRepository,
                                    ICandidateActivityRepository candidateActivityRepository,
                                    IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _candidateActivityRepository = candidateActivityRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _candidateRepository.GetAll("Person").Select(c => new ApiCandidateModel(c)).ToList();
            }, "Candidates Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() =>
            {
                var candidate =  _candidateRepository.Get(id, "Person");
                return new UpdateCandidateModel(candidate);
            }, "Candidate Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(UpdateCandidateModel vm)
        {
            ApiResult<Candidate> apiResult;

            if (ModelState.IsValid)
            {
                if (vm.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var selectedCandidate = _candidateRepository.Get(vm.Id, "Person");
                        selectedCandidate.Email = vm.Email;
                        selectedCandidate.Source = vm.Source;
                        selectedCandidate.Qualification = vm.Qualification;
                        selectedCandidate.TotalExperience = vm.TotalExperience;
                        selectedCandidate.Status = vm.Status;
                        selectedCandidate.Comments = vm.Comments;
                        selectedCandidate.CurrentCTC = vm.CurrentCTC;
                        selectedCandidate.ExpectedCTC = vm.ExpectedCTC;
                        selectedCandidate.DesignationId = vm.CandidateDesignation;
                        selectedCandidate.RecievedOn = vm.RecievedOn;

                        // Update Person 
                        selectedCandidate.Person.FirstName = vm.FirstName;
                        selectedCandidate.Person.MiddleName = vm.MiddleName;
                        selectedCandidate.Person.LastName = vm.LastName;
                        selectedCandidate.Person.Gender = vm.Gender;
                        selectedCandidate.Person.Email = vm.Email;
                        selectedCandidate.Person.SecondaryEmail = vm.SecondaryEmail;
                        selectedCandidate.Person.Organization = vm.Organization;
                        selectedCandidate.Person.Designation = vm.Designation;
                        selectedCandidate.Person.PhoneNo = vm.PhoneNo;
                        selectedCandidate.Person.OfficePhone = vm.OfficePhone;
                        selectedCandidate.Person.Skype = vm.Skype;
                        selectedCandidate.Person.Facebook = vm.Facebook;
                        selectedCandidate.Person.Twitter = vm.Twitter;
                        selectedCandidate.Person.LinkedIn = vm.LinkedIn;
                        selectedCandidate.Person.GooglePlus = vm.GooglePlus;
                        selectedCandidate.Person.Address = vm.Address;
                        selectedCandidate.Person.CommunicationAddress = vm.CommunicationAddress;
                        selectedCandidate.Person.DateOfBirth = vm.DateOfBirth;

                        _candidateRepository.Update(selectedCandidate);
                        _unitOfWork.Commit();
                        return selectedCandidate;
                    }, "Candidate updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var newCandidate = new Candidate
                        {
                            Email = vm.Email,
                            Source = vm.Source,
                            Qualification = vm.Qualification,
                            TotalExperience = vm.TotalExperience,
                            Status = vm.Status,
                            Comments = vm.Comments,
                            CurrentCTC = vm.CurrentCTC,
                            ExpectedCTC = vm.ExpectedCTC,
                            DesignationId = vm.CandidateDesignation,
                            RecievedOn = vm.RecievedOn,
                            Person = new Person
                            {
                                FirstName = vm.FirstName,
                                MiddleName = vm.MiddleName,
                                LastName = vm.LastName,
                                Gender = vm.Gender,
                                Email = vm.Email,
                                SecondaryEmail = vm.SecondaryEmail,
                                Organization = vm.Organization,
                                Designation = vm.Designation,
                                PhoneNo = vm.PhoneNo,
                                OfficePhone = vm.OfficePhone,
                                Skype = vm.Skype,
                                Facebook = vm.Facebook,
                                Twitter = vm.Twitter,
                                LinkedIn = vm.LinkedIn,
                                GooglePlus = vm.GooglePlus,
                                Address = vm.Address,
                                CommunicationAddress = vm.CommunicationAddress,
                                DateOfBirth = vm.DateOfBirth
                            },
                            CreatedByUserId = WebUser.Id
                        };

                        // Update Person 

                        _candidateRepository.Create(newCandidate);
                        _unitOfWork.Commit();
                        return newCandidate;
                    }, "Candidate created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Candidate>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _candidateRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Candidate deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetAllActivitiesForCandidate(int id)
        {
            var activities = _candidateActivityRepository.GetAllBy(r => r.CandidateId == id, o => o.OrderByDescending(f => f.CreatedOn));
            var list = JsonConvert.SerializeObject(activities, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            return list;
        }

        [HttpPost]
        public JsonResult AddNote(CandidateActivityViewModel vm)
        {
            var selectedCandidate = _candidateActivityRepository.Get(vm.CandidateId);
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
    }
}
