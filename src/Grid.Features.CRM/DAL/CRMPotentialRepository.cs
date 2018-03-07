using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMPotentialRepository : GenericRepository<CRMPotential>, ICRMPotentialRepository
    {
        public CRMPotentialRepository(IDbContext context) : base(context)
        {

        }
    }
}