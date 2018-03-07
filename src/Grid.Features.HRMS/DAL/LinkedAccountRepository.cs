using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class LinkedAccountRepository : GenericRepository<LinkedAccount>, ILinkedAccountRepository
    {
        public LinkedAccountRepository(IDbContext context) : base(context)
        {

        }
    }
}