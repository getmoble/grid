using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMContactRepository : GenericRepository<CRMContact>, ICRMContactRepository
    {
        public CRMContactRepository(IDbContext context) : base(context)
        {

        }
    }
}