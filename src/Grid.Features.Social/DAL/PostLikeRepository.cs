using Grid.Features.Common;
using Grid.Features.Social.DAL.Interfaces;
using Grid.Features.Social.Entities;

namespace Grid.Features.Social.DAL
{
    public class PostLikeRepository : GenericRepository<PostLike>, IPostLikeRepository
    {
        public PostLikeRepository(IDbContext context) : base(context)
        {

        }
    }
}