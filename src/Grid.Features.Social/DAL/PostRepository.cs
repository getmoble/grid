using Grid.Features.Common;
using Grid.Features.Social.DAL.Interfaces;
using Grid.Features.Social.Entities;

namespace Grid.Features.Social.DAL
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(IDbContext context) : base(context)
        {

        }
    }
}