
namespace KMS.Core.ViewModels
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set => PageCount = value;
        }

        public int PageSize { get; set; }

        public int RowCount { get; set; }
        public List<int>? RowCounts { get; set; }

        public int FirstRowOnPage => Math.Max(CurrentPage - 2, 1);

        public int LastRowOnPage => Math.Min(CurrentPage + 2, PageCount);
    }

    public class PagedResult<T> : PagedResultBase where T : class
    {
        public PagedResult()
        {
            Results = new List<T>();
        }

        public IList<T> Results { get; set; }
        public T? FillTer { get; set; }
    }
}
