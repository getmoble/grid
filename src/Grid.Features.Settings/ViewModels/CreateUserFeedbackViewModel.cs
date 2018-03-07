using Grid.Features.Common;

namespace Grid.Features.Settings.ViewModels
{
    public class CreateUserFeedbackViewModel: ViewModelBase
    {
        public string Comment { get; set; }
        public string Screenshot { get; set; }
    }
}