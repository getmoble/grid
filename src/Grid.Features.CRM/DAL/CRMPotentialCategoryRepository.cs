using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMPotentialCategoryRepository : GenericRepository<CRMPotentialCategory>, ICRMPotentialCategoryRepository
    {
        public CRMPotentialCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}