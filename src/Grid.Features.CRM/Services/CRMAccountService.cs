using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Services.Interfaces;

namespace Grid.Features.CRM.Services
{
    public class CRMAccountService: ICRMAccountService
    {
        private readonly ICRMAccountRepository _crmAccountRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMAccountService(ICRMAccountRepository crmAccountRepository,
                                 ICRMContactRepository crmContactRepository,
                                 IUnitOfWork unitOfWork)
        {
            _crmAccountRepository = crmAccountRepository;
            _crmContactRepository = crmContactRepository;
            _unitOfWork = unitOfWork;
        }

        public OperationResult<bool> Delete(int id)
        {
            var childAccountsExists = _crmAccountRepository.Any(a => a.ParentId == id);
            if (childAccountsExists)
            {
                return new OperationResult<bool> { Status = false, Message = "We cannot delete Account as it has child accounts" };
                
            }

            var contactsExists = _crmContactRepository.Any(a => a.ParentAccountId == id);
            if (contactsExists)
            {
                return new OperationResult<bool> { Status = false, Message = "We cannot delete Account as it has contacts under it" };
            }

            _crmAccountRepository.Delete(id);
            _unitOfWork.Commit();
            return new OperationResult<bool> { Status = true };
        }
    }
}
