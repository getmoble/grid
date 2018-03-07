using Grid.Features.Common;
using Grid.Features.Payroll.DAL.Interfaces;
using Grid.Features.Payroll.Entities;

namespace Grid.Features.Payroll.DAL
{
    public class SalaryComponentRepository : GenericRepository<SalaryComponent>, ISalaryComponentRepository
    {
        public SalaryComponentRepository(IDbContext context) : base(context)
        {

        }
    }
}