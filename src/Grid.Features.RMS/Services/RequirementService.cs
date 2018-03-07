using System.Collections.Generic;
using System.Net.Mail;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.RMS.DAL.Interfaces;
using Grid.Features.RMS.Services.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.RMS.Services
{
    public class RequirementService : IRequirementService
    {
        private readonly IRequirementTechnologyMapRepository _requirementTechnologyMapRepository;
        private readonly IRequirementActivityRepository _requirementActivityRepository;
        private readonly IRequirementDocumentRepository _requirementDocumentRepository;
        private readonly IRequirementRepository _requirementRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingsService _settingsService;
        private readonly IUserRepository _userRepository;

        public RequirementService(IRequirementTechnologyMapRepository requirementTechnologyMapRepository,
                                  IRequirementActivityRepository requirementActivityRepository,
                                  IRequirementDocumentRepository requirementDocumentRepository,
                                  IRequirementRepository requirementRepository,
                                  ISettingsService settingsService,
                                  IUserRepository userRepository,
                                  IUnitOfWork unitOfWork)
        {
            _requirementTechnologyMapRepository = requirementTechnologyMapRepository;
            _requirementActivityRepository = requirementActivityRepository;
            _requirementDocumentRepository = requirementDocumentRepository;
            _requirementRepository = requirementRepository;
            _settingsService = settingsService;
            _userRepository = userRepository;

            _unitOfWork = unitOfWork;
        }

        public OperationResult<bool> Delete(int id)
        {
            var technologyMaps = _requirementTechnologyMapRepository.GetAllBy(m => m.RequirementId == id);
            foreach (var map in technologyMaps)
            {
                _requirementTechnologyMapRepository.Delete(map);
            }

            var activities = _requirementActivityRepository.GetAllBy(m => m.RequirementId == id);
            foreach (var activity in activities)
            {
                _requirementActivityRepository.Delete(activity);
            }

            var docs = _requirementDocumentRepository.GetAllBy(m => m.RequirementId == id);
            foreach (var doc in docs)
            {
                _requirementDocumentRepository.Delete(doc);
            }

            _requirementRepository.Delete(id);
            _unitOfWork.Commit();

            return new OperationResult<bool> { Status = true };
        }


        public EmailContext ComposeEmailContextForNewRequirement(int requirementId)
        {
            var emailContext = new EmailContext();
            var selectedRequirement = _requirementRepository.Get(requirementId, "Category,CreatedByUser.Person");
            if (selectedRequirement != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[CreatedByUser]", selectedRequirement.CreatedByUser.Person.Name),
                    new PlaceHolder("[Description]", selectedRequirement.Description),
                    new PlaceHolder("[Category]", selectedRequirement.Category.Title),
                    new PlaceHolder("[Url]", $"http://logiticks.gridintra.net/RMS/Requirements/Details/{selectedRequirement.Id}")
                };

                emailContext.Subject = $"New Requirement: {selectedRequirement.Title}";

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    var selectedPOC = _userRepository.Get(settings.POCSettings.SalesDepartmentLevel1, "Person");
                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                        emailContext.ToAddress.Add(pocAddress);
                    }

                    var selectedPOC2 = _userRepository.Get(settings.POCSettings.SalesDepartmentLevel2, "Person");
                    if (selectedPOC2 != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC2.OfficialEmail, selectedPOC2.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }

            return emailContext;
        }

        public EmailContext ComposeEmailContextForRequirementUpdate(int requirementActivityId)
        {
            var emailContext = new EmailContext();
            var selectedRequirementActivity = _requirementActivityRepository.Get(requirementActivityId, "CreatedByUser.Person, Requirement");
            if (selectedRequirementActivity != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[CreatedByUser]", selectedRequirementActivity.CreatedByUser.Person.Name),
                    new PlaceHolder("[RequirementTitle]", selectedRequirementActivity.Title),
                    new PlaceHolder("[Title]", selectedRequirementActivity.Title),
                    new PlaceHolder("[Comment]", selectedRequirementActivity.Comment),
                    new PlaceHolder("[Url]", $"http://logiticks.gridintra.net/RMS/Requirements/Details/{selectedRequirementActivity.Requirement.Id}")
                };

                emailContext.Subject = $"{selectedRequirementActivity.Title}: {selectedRequirementActivity.Requirement.Title}";

                var settings = _settingsService.GetSiteSettings();
                if (settings.POCSettings != null)
                {
                    var selectedPOC = _userRepository.Get(settings.POCSettings.SalesDepartmentLevel1, "Person");
                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.Person.Name);
                        emailContext.ToAddress.Add(pocAddress);
                    }

                    var selectedPOC2 = _userRepository.Get(settings.POCSettings.SalesDepartmentLevel2, "Person");
                    if (selectedPOC2 != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC2.OfficialEmail, selectedPOC2.Person.Name);
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }
            }

            return emailContext;
        }
    }
}
