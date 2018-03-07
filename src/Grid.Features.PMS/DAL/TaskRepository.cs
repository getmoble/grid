using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        public TaskRepository(IDbContext context) : base(context)
        {

        }
    }
}