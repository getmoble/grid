using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class CertificationRepository : GenericRepository<Certification>, ICertificationRepository
    {
        public CertificationRepository(IDbContext context) : base(context)
        {

        }
    }
}