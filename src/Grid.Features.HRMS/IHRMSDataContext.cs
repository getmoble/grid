using System.Data.Entity;
using Grid.Features.Common;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS
{
    public interface IHRMSDataContext : IDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Employee> Employees { get; set; }
    }
}
