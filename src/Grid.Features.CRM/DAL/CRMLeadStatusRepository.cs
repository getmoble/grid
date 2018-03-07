using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMLeadStatusRepository : GenericRepository<CRMLeadStatus>, ICRMLeadStatusRepository
    {
        public CRMLeadStatusRepository(IDbContext context) : base(context)
        {

        }
    }
}