using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMLeadCategoryRepository : GenericRepository<CRMLeadCategory>, ICRMLeadCategoryRepository
    {
        public CRMLeadCategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}