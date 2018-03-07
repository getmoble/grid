using Grid.Features.Common;
using Grid.Features.Social.DAL.Interfaces;
using Grid.Features.Social.Entities;

namespace Grid.Features.Social.DAL
{
    public class PostCommentRepository : GenericRepository<PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(IDbContext context) : base(context)
        {

        }
    }
}