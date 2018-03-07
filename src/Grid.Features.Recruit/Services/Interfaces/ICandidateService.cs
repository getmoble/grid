using Grid.Features.Common;

namespace Grid.Features.Recruit.Services.Interfaces
{
    public interface ICandidateService
    {
        OperationResult<bool> Delete(int id);
    }
}
