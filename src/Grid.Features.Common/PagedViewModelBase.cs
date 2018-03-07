namespace Grid.Features.Common
{
    public class PagedViewModelBase: ViewModelBase
    {
        public int? page { get; set; }
        public int PageSize { get; set; }

        public int GetPageNo()
        {
            return page ?? 1;
        }
        public PagedViewModelBase()
        {
            PageSize = 100;
        }
    }
}