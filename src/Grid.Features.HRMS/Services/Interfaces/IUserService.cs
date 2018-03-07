using Grid.Providers.Email;

namespace Grid.Features.HRMS.Services.Interfaces
{
    public interface IUserService
    {
        EmailContext ComposeEmailContextForBirthdayReminder();
    }
}
