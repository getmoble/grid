using Grid.Data;

namespace Grid
{
    public static class DbContextFactory
    {
        public static GridDataContext Create()
        {
            return new GridDataContext();
        }
    }
}