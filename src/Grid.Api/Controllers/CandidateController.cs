using System;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;
using Grid.Features.Recruit.Entities.Enums;
using Grid.Infrastructure.Filters;

namespace Grid.Api.Controllers
{
    [APIIdentityInjector]
    public class CandidateController : GridBaseController
    {
        private readonly IPersonRepository _personRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidateController(IPersonRepository personRepository,
                                   ICandidateRepository candidateRepository,
                                   IUnitOfWork unitOfWork)
        {
            _personRepository = personRepository;
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult Create(CandidateModel model)
        {
            try
            {
                var newPerson = new Person
                {
                    Email = model.Email,
                    PhoneNo = model.Phone,
                    FirstName = !string.IsNullOrEmpty(model.FirstName) ? model.FirstName : "Unknown",
                    LastName = !string.IsNullOrEmpty(model.LastName) ? model.LastName : "Unknown"
                };

                _personRepository.Create(newPerson);

                var newCandidate = new Candidate
                {
                    Source = model.Source,
                    Status = CandidateStatus.New,
                    PersonId = newPerson.Id,
                    RecievedOn = DateTime.UtcNow,
                    CreatedByUserId = model.CreatedByUserId,
                    Comments = model.Comments
                };

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

                var result = new ApiResult<bool>
                {
                    Status = true,
                    Message = "Success"
                };

                return Json(result);
            }
            catch (Exception ex)
            {
                var result = new ApiResult<bool>
                {
                    Status = false,
                    Message = ex.Message
                };

                return Json(result);
            }
        }
    }
}