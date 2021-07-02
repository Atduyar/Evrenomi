using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.DataAccess.Abstract;
using Core.DataAccess.Concrete;
using Core.DataAccess.Concrete.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfBlogDal:EfEntityRepositoryBase<Blog,VikingContext>,IBlogDal
    {
        public List<Blog> GetSearchList(List<string> filters = null)
        {
            using (var context = new VikingContext())
            {
                if (filters == null)
                {
                    return context.Set<Blog>().ToList();
                }
                else
                {
                    filters.Reverse();//ilk sıradaki öncelikli
                    var query = Enumerable.Empty<Blog>().AsQueryable();//context.Set<Blog>().AsQueryable();
                    foreach (var filter in filters)
                    {
                        //query = query.Where(b => b.BlogTitle.Contains(filter));
                        query = query.Concat(context.Set<Blog>().Where(b => b.BlogTitle.Contains(filter)));
                    }
                    //var sirala = query.GroupBy(x => x)
                    //    .Where(g => g.Count() > 1)
                    //    .Select(y => new { Element = y.Key, Counter = y.Count() })
                    //    .ToList();
                    var blogs = query.GroupBy(x => x)
                        .Select(y => new {Element = y.Key, Counter = y.Count()})
                        .OrderBy(w => w.Counter)
                        .Select(z => z.Element)
                        .ToList(); 

                    return blogs;
                }
                
            }
        }

        public PagedList<Blog> GetPaged(PageFilter pageFliter,Expression<Func<Blog, bool>> filter = null)
        {
            using (var context = new VikingContext())
            {//(((Status.Neno ^ b.BlogStatus) & Status.DontView) == 0)
                return PagedList<Blog>.ToPagedList(context.Set<Blog>().Where(filter),
                    pageFliter.PageNumber,
                    pageFliter.PageSize, filter);
            }
        }
        public List<Tag> GetTag(int blogId)
        {
            using (var context = new VikingContext())
            {
                var result = from tag in context.Tags
                    join blogTag in context.BlogTags
                        on tag.Id equals blogTag.TagId
                    where blogTag.BlogId == blogId
                             select new Tag { Id = tag.Id, Name = tag.Name };
                return result.ToList();
            }
        }
    }
}