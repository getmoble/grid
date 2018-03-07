using Grid.Features.Common;
using Grid.Providers.Email;

namespace Grid.Features.RMS.Services.Interfaces
{
    public interface IRequirementService
    {
        OperationResult<bool> Delete(int id);
        EmailContext ComposeEmailContextForNewRequirement(int requirementId);
        EmailContext ComposeEmailContextForRequirementUpdate(int requirementId);
    }
}
