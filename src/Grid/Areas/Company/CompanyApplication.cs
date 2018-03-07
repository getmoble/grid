using Grid.Providers.Plugin;

namespace Grid.Areas.Company
{
    public class CompanyApplication: IGridApplication
    {
        public ApplicationInfo GetApplicationInfo()
        {
            return new ApplicationInfo
            {
                Category = "Company",
                Title = "Company",
                Description = "App to manage company related information",
                Icon = "fa fa-rocket",
                Url = "/Company/Locations"
            };
        }
    }
}