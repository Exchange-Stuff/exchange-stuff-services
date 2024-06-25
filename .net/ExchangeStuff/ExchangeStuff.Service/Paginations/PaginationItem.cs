namespace ExchangeStuff.Service.Paginations
{
    public class PaginationItem<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public List<T> ListItem { get; set; } = new List<T>();
        public PaginationItem(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            ListItem.AddRange(items);
        }

        public static PaginationItem<T> ToPagedList(List<T> source, int? pageIndex = null!, int? pageSize = null!)
        {
            int index = 0;
            int size = 10;
            if (pageIndex.HasValue && pageSize.HasValue)
            {
                index = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                size = pageSize.Value > 0 ? pageSize.Value : 10;
            }
            var count = source.Count();
            var items = source.Skip(index * size).Take(size).ToList();
            pageIndex = pageIndex ==null || pageIndex.HasValue&& pageIndex.Value<1 ? 1 : pageIndex;
            return new PaginationItem<T>(items, count, (int)pageIndex!, size);
        }
    }
}
