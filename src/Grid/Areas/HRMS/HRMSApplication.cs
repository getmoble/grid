using Grid.Providers.Plugin;

namespace Grid.Areas.HRMS
{
    public class HRMSApplication : IGridApplication
    {
        public ApplicationInfo GetApplicationInfo()
        {
            return new ApplicationInfo
            {
                Category = "HRMS",
                Title = "HRMS",
                Description = "App to manage company related HRMS",
                Icon = "fa fa-rocket",
                Url = "/"
            };
        }
    }
}