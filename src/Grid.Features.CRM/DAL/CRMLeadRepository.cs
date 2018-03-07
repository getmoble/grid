using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMLeadRepository : GenericRepository<CRMLead>, ICRMLeadRepository
    {
        public CRMLeadRepository(IDbContext context) : base(context)
        {

        }
    }
}