using Grid.Features.Common;


namespace Grid.ViewModels
{
    public class ScheduledJobViewModel: ViewModelBase
    {
        public string Name { get; set; }

        public string CronExpression { get; set; }

        public string Url { get; set; }
    }
}