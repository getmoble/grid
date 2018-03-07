using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class RoundRepository : GenericRepository<Round>, IRoundRepository
    {
        public RoundRepository(IDbContext context) : base(context)
        {

        }
    }
}