using Grid.Features.Common;

namespace Grid.Features.CRM.Services.Interfaces
{
    public interface ICRMContactService
    {
        OperationResult<bool> Delete(int id);
    }
}
