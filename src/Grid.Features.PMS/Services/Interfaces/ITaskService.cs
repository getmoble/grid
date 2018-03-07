using Grid.Providers.Email;

namespace Grid.Features.PMS.Services.Interfaces
{
    public interface ITaskService
    {
        EmailContext ComposeEmailContextForTaskCreated(int taskId);
        EmailContext ComposeEmailContextForTaskUpdated(int taskId);
        EmailContext ComposeEmailContextForTaskMissed(int taskId);
    }
}
