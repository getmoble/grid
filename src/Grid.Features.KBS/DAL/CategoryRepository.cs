using Grid.Features.Common;
using Grid.Features.KBS.DAL.Interfaces;
using Grid.Features.KBS.Entities;

namespace Grid.Features.KBS.DAL
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDbContext context) : base(context)
        {

        }
    }
}