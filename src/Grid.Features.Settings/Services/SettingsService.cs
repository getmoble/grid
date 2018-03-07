using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Entities;
using Grid.Features.Settings.Models;
using Grid.Features.Settings.Services.Interfaces;
using Newtonsoft.Json;

namespace Grid.Features.Settings.Services
{
    public class SettingsService: ISettingsService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SettingsService(ISettingRepository settingRepository,
                               IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        public SettingsModel GetSiteSettings()
        {
            var selectedSettings = _settingRepository.GetBy(s => s.Key == "SiteSettings");
            if (selectedSettings != null)
            {
                var deserialized = JsonConvert.DeserializeObject<SettingsModel>(selectedSettings.Value);
                return deserialized;
            }

            return new SettingsModel();
        }

        public bool UpdateSiteSettings(SettingsModel updatedSettings)
        {
            var serialized = JsonConvert.SerializeObject(updatedSettings);
            var selectedSettings = _settingRepository.GetBy(s => s.Key == "SiteSettings");
            if (selectedSettings != null)
            {
                selectedSettings.Value = serialized;
                _settingRepository.Update(selectedSettings);
                _unitOfWork.Commit();
            }
            else
            {
                var settings = new Setting
                {
                    Key = "SiteSettings",
                    Value = serialized
                };

                _settingRepository.Create(settings);
                _unitOfWork.Commit();
            }

            return true;
        }
    }
}