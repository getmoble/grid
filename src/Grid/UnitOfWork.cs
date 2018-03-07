using Grid.Features.Common;

namespace Grid
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }
    }
}
