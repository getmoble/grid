using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectBillingRepository : GenericRepository<ProjectBilling>, IProjectBillingRepository
    {
        public ProjectBillingRepository(IDbContext context) : base(context)
        {

        }
    }
}