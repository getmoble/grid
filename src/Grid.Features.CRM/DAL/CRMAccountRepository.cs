using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.CRM.Entities;

namespace Grid.Features.CRM.DAL
{
    public class CRMAccountRepository : GenericRepository<CRMAccount>, ICRMAccountRepository
    {
        public CRMAccountRepository(IDbContext context) : base(context)
        {

        }
    }
}