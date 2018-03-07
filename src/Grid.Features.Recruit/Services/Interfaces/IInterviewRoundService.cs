using Grid.Providers.Email;

namespace Grid.Features.Recruit.Services.Interfaces
{
    public interface IInterviewRoundService
    {
        EmailContext ComposeEmailContextForInterviewScheduled(int interviewId);
        EmailContext ComposeEmailContextForInterviewReminder(int interviewId);
        EmailContext ComposeEmailContextForInterviewFeedbackReminder(int interviewId);
    }
}
