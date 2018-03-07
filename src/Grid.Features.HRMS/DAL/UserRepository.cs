using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext context) : base(context)
        {

        }
    }
}
