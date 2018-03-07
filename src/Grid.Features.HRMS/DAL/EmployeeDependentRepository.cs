using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class EmployeeDependentRepository : GenericRepository<EmployeeDependent>, IEmployeeDependentRepository
    {
        public EmployeeDependentRepository(IDbContext context) : base(context)
        {

        }
    }
}