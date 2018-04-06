using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectBillingCorrectionRepository : GenericRepository<ProjectBillingCorrection>, IProjectBillingCorrectionRepository
    {
        public ProjectBillingCorrectionRepository(IDbContext context) : base(context)
        {

        }
    }
}