using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Features.HRMS.DAL
{
    public class UserDocumentRepository : GenericRepository<UserDocument>, IUserDocumentRepository
    {
        public UserDocumentRepository(IDbContext context) : base(context)
        {

        }
    }
}