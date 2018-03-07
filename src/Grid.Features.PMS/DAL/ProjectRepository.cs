using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using System.Collections.Generic;

namespace Grid.Features.PMS.DAL
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {       
        public ProjectRepository(IDbContext context) : base(context)
        {
         
        }        
    }
}
