using Grid.Features.Settings.Models;

namespace Grid.Features.Settings.Services.Interfaces
{
    public interface ISettingsService
    {
        SettingsModel GetSiteSettings();
        bool UpdateSiteSettings(SettingsModel settings);
    }
}
