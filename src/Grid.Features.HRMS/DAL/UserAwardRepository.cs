using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class UserAwardRepository : GenericRepository<UserAward>, IUserAwardRepository
    {
        public UserAwardRepository(IDbContext context) : base(context)
        {

        }
    }
}