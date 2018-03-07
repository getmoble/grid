using Grid.Data;

namespace Grid
{
    public interface IDbContextFactory
    {
        GridDataContext Create();
    }
}