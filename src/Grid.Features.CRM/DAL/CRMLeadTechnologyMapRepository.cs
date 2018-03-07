using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMLeadTechnologyMapRepository : GenericRepository<CRMLeadTechnologyMap>, ICRMLeadTechnologyMapRepository
    {
        public CRMLeadTechnologyMapRepository(IDbContext context) : base(context)
        {

        }
    }
}