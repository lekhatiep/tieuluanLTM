using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Common.Paging
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public int TotalPages { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalPages = (int)Math.Ceiling((double)count / pageSize);
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> querySource, int pageNumber, int pageSize)
        {
            var count = querySource.Count();
            var items = querySource
                        .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> querySource, int pageNumber, int pageSize)
        {
            var count = querySource.Count();
            var items = querySource
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
