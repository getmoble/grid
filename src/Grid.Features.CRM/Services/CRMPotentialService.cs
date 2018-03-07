using System;
using System.Linq;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;
using Grid.Features.CRM.Entities.Enums;
using Grid.Features.CRM.Services.Interfaces;

namespace Grid.Features.CRM.Services
{
    public class CRMPotentialService: ICRMPotentialService
    {
        private readonly ICRMLeadRepository _crmLeadRepository;
        private readonly ICRMLeadTechnologyMapRepository _crmLeadTechnologyMapRepository;
        private readonly ICRMLeadActivityRepository _crmLeadActivityRepository;
        private readonly ICRMAccountRepository _crmAccountRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly ICRMPotentialRepository _crmPotentialRepository;
        private readonly ICRMPotentialTechnologyMapRepository _crmPotentialTechnologyMapRepository;
        private readonly ICRMPotentialActivityRepository _crmPotentialActivityRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ICRMLeadService _crmLeadService;

        public CRMPotentialService(ICRMLeadRepository crmLeadRepository,
                                       ICRMLeadTechnologyMapRepository crmLeadTechnologyMapRepository,
                                       ICRMLeadActivityRepository crmLeadActivityRepository,
                                       ICRMAccountRepository crmAccountRepository,
                                       ICRMContactRepository crmContactRepository,
                                       ICRMPotentialRepository crmPotentialRepository,
                                       ICRMPotentialTechnologyMapRepository crmPotentialTechnologyMapRepository,
                                       ICRMPotentialActivityRepository crmPotentialActivityRepository,
                                       IUnitOfWork unitOfWork,
                                       ICRMLeadService crmLeadService)
        {
            _crmLeadRepository = crmLeadRepository;
            _crmLeadActivityRepository = crmLeadActivityRepository;
            _crmLeadTechnologyMapRepository = crmLeadTechnologyMapRepository;
            _crmAccountRepository = crmAccountRepository;
            _crmContactRepository = crmContactRepository;
            _crmPotentialRepository = crmPotentialRepository;
            _crmPotentialTechnologyMapRepository = crmPotentialTechnologyMapRepository;
            _crmPotentialActivityRepository = crmPotentialActivityRepository;
            _unitOfWork = unitOfWork;

            _crmLeadService = crmLeadService;
        }

        public bool Convert(bool createAccount, bool createPotential, int id, int assignedToUserId, int? categoryId, double? expectedAmount, DateTime? expectedCloseDate, string description, DateTime? enquiredOn, int salesStage, int createdByUserId)
        {
            var selectedLead = _crmLeadRepository.GetBy(l => l.Id == id, "Person");
            if (selectedLead != null)
            {
                CRMAccount selectedOrganization = null;

                if (createAccount)
                {
                    // Create the Account First
                    var organization = selectedLead.Person.Organization;

                    if (!string.IsNullOrEmpty(organization))
                    {
                        selectedOrganization = _crmAccountRepository.GetBy(a => a.Title == organization);
                        if (selectedOrganization == null)
                        {
                            selectedOrganization = new CRMAccount
                            {
                                Title = organization,
                                EmployeeCount = EmployeeCount.One2Ten,
                                AssignedToEmployeeId = assignedToUserId,
                                CreatedByUserId = createdByUserId
                            };

                            _crmAccountRepository.Create(selectedOrganization);
                            _unitOfWork.Commit();
                        }
                    }
                }
                
                // Create Contact
                var contact = new CRMContact
                {
                    PersonId = selectedLead.PersonId,
                    CreatedByUserId = createdByUserId
                };

                // Assign the account Id
                if (selectedOrganization != null)
                {
                    contact.ParentAccountId = selectedOrganization.Id;
                }

                _crmContactRepository.Create(contact);
                _unitOfWork.Commit();

                if (createPotential)
                {
                    // Create the Potential
                    var potential = new CRMPotential
                    {
                        CategoryId = categoryId,
                        ExpectedAmount = expectedAmount,
                        ExpectedCloseDate = expectedCloseDate,
                        Description = description,
                        EnquiredOn = enquiredOn,
                        SalesStageId = salesStage,
                        AssignedToUserId = assignedToUserId,
                        ContactId = contact.Id,
                        CreatedByUserId = createdByUserId
                    };

                    _crmPotentialRepository.Create(potential);
                    _unitOfWork.Commit();

                    // Move all Lead Technologies to Potential Technologies 
                    var technologies = _crmLeadTechnologyMapRepository.GetAllBy(m => m.LeadId == selectedLead.Id).ToList();
                    foreach (var technology in technologies)
                    {
                        var technologyMap = new CRMPotentialTechnologyMap
                        {
                            PotentialId = potential.Id,
                            TechnologyId = technology.TechnologyId
                        };

                        _crmPotentialTechnologyMapRepository.Create(technologyMap);
                    }

                    // Move all Lead Activities to Potential Activities 
                    var activities = _crmLeadActivityRepository.GetAllBy(m => m.CRMLeadId == selectedLead.Id).ToList();
                    foreach (var activity in activities)
                    {
                        var potentialActivity = new CRMPotentialActivity
                        {
                            CRMPotentialId = potential.Id,
                            Title = activity.Title,
                            Comment = activity.Comment,
                            CreatedOn = activity.CreatedOn,
                            CreatedByUserId = createdByUserId
                        };

                        _crmPotentialActivityRepository.Create(potentialActivity);
                    }

                    _unitOfWork.Commit();
                }
                
                // Delete the Lead
                _crmLeadService.Delete(id);

                return true;
            }

            return false;
        }

        public bool Delete(int id)
        {
            // Check whether Potential exists or not ? 
            var crmPotential = _crmPotentialRepository.Get(id);
            if (crmPotential != null)
            {
                // Delete all Potential Activities
                var activities = _crmPotentialActivityRepository.GetAllBy(m => m.CRMPotentialId == id).ToList();
                foreach (var activity in activities)
                {
                    _crmPotentialActivityRepository.Delete(activity);
                }
                
                // Delete all Potential Technology Maps
                var technologies = _crmPotentialTechnologyMapRepository.GetAllBy(m => m.PotentialId == id).ToList();
                foreach (var technology in technologies)
                {
                    _crmPotentialTechnologyMapRepository.Delete(technology);
                }

                _unitOfWork.Commit();

                _crmPotentialRepository.Delete(id);
                _unitOfWork.Commit();

                return true;
            }

            return false;
        }
    }
}
