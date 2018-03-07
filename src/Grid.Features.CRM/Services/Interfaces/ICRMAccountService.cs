using Grid.Features.Common;

namespace Grid.Features.CRM.Services.Interfaces
{
    public interface ICRMAccountService
    {
        OperationResult<bool> Delete(int id);
    }
}
