namespace API.Common.Paging
{
    public class PagingRequetBase
    {
        const int maxPageSize = 50;
        private int _pageNumber { get; set; }
        private int _pageSize { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value < 0) ? 1 : value;
        }
    }
}
