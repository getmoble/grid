using Grid.Features.Common;
using Grid.Features.Settings.DAL.Interfaces;
using Grid.Features.Settings.Entities;

namespace Grid.Features.Settings.DAL
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(IDbContext context) : base(context)
        {

        }
    }
}
