using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class UserTechnologyMapRepository : GenericRepository<UserTechnologyMap>, IUserTechnologyMapRepository
    {
        public UserTechnologyMapRepository(IDbContext context) : base(context)
        {

        }
    }
}