using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class TaskActivityRepository : GenericRepository<TaskActivity>, ITaskActivityRepository
    {
        public TaskActivityRepository(IDbContext context) : base(context)
        {

        }
    }
}