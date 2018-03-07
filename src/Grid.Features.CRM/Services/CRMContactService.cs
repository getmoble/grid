using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Services.Interfaces;

namespace Grid.Features.CRM.Services
{
    public class CRMContactService: ICRMContactService
    {
        private readonly ICRMPotentialRepository _crmPotentialRepository;
        private readonly ICRMContactRepository _crmContactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMContactService(ICRMPotentialRepository crmPotentialRepository,
                                 ICRMContactRepository crmContactRepository,
                                 IUnitOfWork unitOfWork)
        {
            _crmPotentialRepository = crmPotentialRepository;
            _crmContactRepository = crmContactRepository;
            _unitOfWork = unitOfWork;
        }

        public OperationResult<bool> Delete(int id)
        {
            var potentialExists = _crmPotentialRepository.Any(a => a.ContactId == id);
            if (!potentialExists)
            {
                _crmContactRepository.Delete(id);
                _unitOfWork.Commit();
                return new OperationResult<bool> { Status = true };
            }

            return new OperationResult<bool> { Status = false, Message = "We cannot delete Contact as it has linked Potential" };
        }
    }
}
