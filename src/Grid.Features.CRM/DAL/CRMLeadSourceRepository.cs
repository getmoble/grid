using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMLeadSourceRepository : GenericRepository<CRMLeadSource>, ICRMLeadSourceRepository
    {
        public CRMLeadSourceRepository(IDbContext context) : base(context)
        {

        }
    }
}