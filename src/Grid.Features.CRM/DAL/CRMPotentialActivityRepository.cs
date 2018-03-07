using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMPotentialActivityRepository : GenericRepository<CRMPotentialActivity>, ICRMPotentialActivityRepository
    {
        public CRMPotentialActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}