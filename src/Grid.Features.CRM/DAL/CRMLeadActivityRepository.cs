using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMLeadActivityRepository : GenericRepository<CRMLeadActivity>, ICRMLeadActivityRepository
    {
        public CRMLeadActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}