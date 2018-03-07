using Grid.Providers.Plugin;

namespace Grid.Areas.CRM
{
    public class CRMApplication : IGridApplication
    {
        public ApplicationInfo GetApplicationInfo()
        {
            return new ApplicationInfo
            {
                Category = "CRM",
                Title = "CRM",
                Description = "App to manage company related CRM",
                Icon = "fa fa-rocket",
                Url = "/"
            };
        }
    }
}