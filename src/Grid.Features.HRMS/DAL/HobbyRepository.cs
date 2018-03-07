using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class HobbyRepository : GenericRepository<Hobby>, IHobbyRepository
    {
        public HobbyRepository(IDbContext context) : base(context)
        {

        }
    }
}