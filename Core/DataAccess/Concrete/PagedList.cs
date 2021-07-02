using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.DataAccess.Concrete
{
    public class PagedList<T>:List<T>
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
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null)
        {
            var count = source.Count();

            List<T> items;
            if (count < (pageNumber) * pageSize)
            {
                items = source.Skip(0).Take((count - ((pageNumber - 1) * pageSize))).ToList();
            }
            else
            {
                items = source.Skip((count - pageSize) - ((pageNumber - 1) * pageSize)).Take(pageSize).ToList();
            }
            //var items = source.Reverse().Skip(((pageNumber - 1) * pageSize)).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}