using System.Collections.Generic;
using System.Linq;

namespace Sarwa.Core.Data.EFCore.Paging
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public PagedList(IQueryable<T> source, int page, int pageSize)
        { 
            TotalCount = source.Count();
            TotalPages = (int)System.Math.Ceiling((double)TotalCount / pageSize);
            PageSize = pageSize;
            PageIndex = page;
            AddRange(source.Skip(pageSize * (page - 1)).Take(pageSize).ToList());
        }
        
        public PagedList(IEnumerable<T> source, int page, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = (int)System.Math.Ceiling((double)TotalCount / pageSize);
            PageSize = pageSize;
            PageIndex = page;
            AddRange(source.Skip(pageSize * (page - 1)).Take(pageSize).ToList());
        }

        public PagedList(IEnumerable<T> source, int page, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = (int)System.Math.Ceiling((double)TotalCount / pageSize);
            PageSize = pageSize;
            PageIndex = page;
            AddRange(source);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage => PageIndex > 0;

        public bool HasNextPage => (PageIndex + 1 < TotalPages);

        public bool IsFirstPage => !HasPreviousPage;

        public bool IsLastPage => !HasNextPage;
    }
}
