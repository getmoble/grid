using Grid.Features.Common;
using Grid.Features.Payroll.DAL.Interfaces;
using Grid.Features.Payroll.Entities;

namespace Grid.Features.Payroll.DAL
{
    public class EmployeeSalaryRepository : GenericRepository<EmployeeSalary>, IEmployeeSalaryRepository
    {
        public EmployeeSalaryRepository(IDbContext context) : base(context)
        {

        }
    }
}