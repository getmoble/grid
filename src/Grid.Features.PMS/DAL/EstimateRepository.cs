using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class EstimateRepository : GenericRepository<Estimate>, IEstimateRepository
    {
        public EstimateRepository(IDbContext context) : base(context)
        {

        }
    }
}
