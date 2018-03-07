using System.Linq;
using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Services.Interfaces;

namespace Grid.Features.CRM.Services
{
    public class CRMLeadService: ICRMLeadService
    {
        private readonly ICRMLeadRepository _crmLeadRepository;
        private readonly ICRMLeadActivityRepository _crmLeadActivityRepository;
        private readonly ICRMLeadTechnologyMapRepository _crmLeadTechnologyMapRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CRMLeadService(ICRMLeadRepository crmLeadRepository,
                              ICRMLeadActivityRepository crmLeadActivityRepository,
                              ICRMLeadTechnologyMapRepository crmLeadTechnologyMapRepository,
                              IUnitOfWork unitOfWork)
        {
            _crmLeadRepository = crmLeadRepository;
            _crmLeadActivityRepository = crmLeadActivityRepository;
            _crmLeadTechnologyMapRepository = crmLeadTechnologyMapRepository;
            _unitOfWork = unitOfWork;
        }

        public bool Delete(int id)
        {
            // Check whether lead exists or not ? 
            var crmLead = _crmLeadRepository.Get(id);
            if (crmLead != null)
            {
                // Delete all Lead Activities
                var activities = _crmLeadActivityRepository.GetAllBy(m => m.CRMLeadId == id).ToList();
                foreach (var activity in activities)
                {
                    _crmLeadActivityRepository.Delete(activity);
                }
                
                // Delete all Lead Technology Maps
                var technologies = _crmLeadTechnologyMapRepository.GetAllBy(m => m.LeadId == id).ToList();
                foreach (var technology in technologies)
                {
                    _crmLeadTechnologyMapRepository.Delete(technology);
                }

                _unitOfWork.Commit();

                _crmLeadRepository.Delete(id);
                _unitOfWork.Commit();

                return true;
            }

            return false;
        }
    }
}
