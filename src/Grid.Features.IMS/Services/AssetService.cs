using System.Collections.Generic;
using System.Net.Mail;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.IMS.DAL.Interfaces;
using Grid.Features.IMS.Services.Interfaces;
using Grid.Features.Settings.Services.Interfaces;
using Grid.Providers.Email;

namespace Grid.Features.IMS.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISettingsService _settingsService;
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AssetService(IAssetRepository assetRepository,
                            IUnitOfWork unitOfWork,
                            IUserRepository userRepository,
                            IEmployeeRepository employeeRepository,
                            ISettingsService settingsService)
        {
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
            _settingsService = settingsService;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
        }

        public EmailContext ComposeEmailContextForAssetStateChanged(int assetId)
        {
            var emailContext = new EmailContext();
            var selectedAsset = _assetRepository.Get(assetId, "AssetCategory,AllocatedEmployee.User.Person");
            var settings = _settingsService.GetSiteSettings();
            if (selectedAsset != null)
            {
                emailContext.PlaceHolders = new List<PlaceHolder>
                {
                    new PlaceHolder("[AssetTitle]", selectedAsset.Title),
                    new PlaceHolder("[AssetSerial]", selectedAsset.SerialNumber),
                    new PlaceHolder("[AssetTag]", selectedAsset.TagNumber),
                    new PlaceHolder("[AssetCategory]", selectedAsset.AssetCategory.Title),
                    new PlaceHolder("[AssetState]", selectedAsset.State.ToString())
                };

                if (selectedAsset.AllocatedEmployee != null)
                {
                    emailContext.PlaceHolders.Add(new PlaceHolder("[Name]", selectedAsset.AllocatedEmployee.User.Person.Name));
                    emailContext.ToAddress.Add(new MailAddress(selectedAsset.AllocatedEmployee.OfficialEmail, selectedAsset.AllocatedEmployee.User.Person.Name));
                }

                // Use POC settings
                if (settings.POCSettings != null)
                {
                    var pointOfContact = settings.POCSettings.ITDepartmentLevel1;
                    //var selectedPOC = _userRepository.Get(pointOfContact, "Person");
                    var selectedPOC = _employeeRepository.Get(pointOfContact, "User,User.Person");

                    if (selectedPOC != null)
                    {
                        var pocAddress = new MailAddress(selectedPOC.OfficialEmail, selectedPOC.User.Person.Name);

                        // No Assigned Person
                        if (selectedAsset.AllocatedEmployee == null)
                        {
                            emailContext.PlaceHolders.Add(new PlaceHolder("[Name]", selectedPOC.User.Person.Name));
                            emailContext.ToAddress.Add(pocAddress);
                        }

                        // Copy POC
                        emailContext.CcAddress.Add(pocAddress);
                    }
                }

                emailContext.Subject = "Asset Modification";
                
            }

            return emailContext;
        }
    }
}
