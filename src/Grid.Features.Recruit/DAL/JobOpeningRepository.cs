using Grid.Features.Common;
using Grid.Features.Recruit.DAL.Interfaces;
using Grid.Features.Recruit.Entities;

namespace Grid.Features.Recruit.DAL
{
    public class JobOpeningRepository : GenericRepository<JobOpening>, IJobOpeningRepository
    {
        public JobOpeningRepository(IDbContext context) : base(context)
        {

        }
    }
}