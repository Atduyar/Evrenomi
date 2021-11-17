using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface IBlogDal:IEntityRepository<Blog>,IEntityPagedList<Blog>
    {
        List<Blog> GetSearchList(List<string> text = null, Expression<Func<Blog, bool>> filter = null);
        List<Tag> GetTag(int blogId);
    }
}