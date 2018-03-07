using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;

namespace Grid.Features.IT.DAL
{
    public class SoftwareRepository : GenericRepository<Software>, ISoftwareRepository
    {
        public SoftwareRepository(IDbContext context) : base(context)
        {

        }
    }
}