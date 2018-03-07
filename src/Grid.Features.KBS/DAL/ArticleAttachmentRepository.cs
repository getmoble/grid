using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;

namespace Grid.Features.KBS.DAL
{
    public class ArticleAttachmentRepository : GenericRepository<ArticleAttachment>, IArticleAttachmentRepository
    {
        public ArticleAttachmentRepository(IDbContext context) : base(context)
        {

        }
    }
}