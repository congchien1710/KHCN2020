using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Data.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        public static PagedList<T> ToPagedList(IQueryable<T> source, int? pageNumber = 1, int? pageSize = 10)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return new PagedList<T>(items, count, pageNumber.Value, pageSize.Value);
        }

        public async static Task<PagedList<T>> ToPagedListAsync(IQueryable<T> source, int? pageNumber, int? pageSize)
        {
            var count = await source.CountAsync();
            var items = source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return new PagedList<T>(items, count, pageNumber.Value, pageSize.Value);
        }
    }
}