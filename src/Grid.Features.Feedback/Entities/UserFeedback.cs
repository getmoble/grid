using Grid.Features.Feedback.Entities.Enums;
using Grid.Features.HRMS;

namespace Grid.Features.Feedback.Entities
{
    public class UserFeedback : UserCreatedEntityBase
    {
        public string Content { get; set; }
        public string Screenshot { get; set; }
        public FeedbackState State { get; set; }
        public string Comments { get; set; }
    }
}