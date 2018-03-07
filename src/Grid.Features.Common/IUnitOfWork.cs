namespace Grid.Features.Common
{
    public interface IUnitOfWork
    {
        int Commit();
    }
}
