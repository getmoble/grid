using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class ReferalRepository : GenericRepository<Referal>, IReferalRepository
    {
        public ReferalRepository(IDbContext context) : base(context)
        {

        }
    }
}