using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class EmergencyContactRepository : GenericRepository<EmergencyContact>, IEmergencyContactRepository
    {
        public EmergencyContactRepository(IDbContext context) : base(context)
        {

        }
    }
}