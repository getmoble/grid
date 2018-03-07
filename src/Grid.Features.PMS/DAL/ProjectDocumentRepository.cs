using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;

namespace Grid.Features.PMS.DAL
{
    public class ProjectDocumentRepository : GenericRepository<ProjectDocument>, IProjectDocumentRepository
    {
        public ProjectDocumentRepository(IDbContext context) : base(context)
        {

        }
    }
}