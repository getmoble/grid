using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class AwardRepository : GenericRepository<Award>, IAwardRepository
    {
        public AwardRepository(IDbContext context) : base(context)
        {

        }
    }
}