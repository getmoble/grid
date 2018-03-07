using Grid.Providers.Plugin;

namespace Grid.Areas.Accounting
{
    public class AccountingApplication : IGridApplication
    {
        public ApplicationInfo GetApplicationInfo()
        {
            return new ApplicationInfo
            {
                Category = "Accounting",
                Title = "Accounting",
                Description = "App to manage company related accounting",
                Icon = "fa fa-rocket",
                Url = "/"
            };
        }
    }
}