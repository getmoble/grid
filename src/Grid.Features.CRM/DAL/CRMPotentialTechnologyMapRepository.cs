using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMPotentialTechnologyMapRepository : GenericRepository<CRMPotentialTechnologyMap>, ICRMPotentialTechnologyMapRepository
    {
        public CRMPotentialTechnologyMapRepository(IDbContext context) : base(context)
        {

        }
    }
}
