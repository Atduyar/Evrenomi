using System;
using System.Linq.Expressions;
using Core.DataAccess.Concrete;

namespace Core.DataAccess.Abstract
{
    public interface IEntityPagedList<T>
    {
        PagedList<T> GetPaged(PageFilter pageFliter, Expression<Func<T, bool>> filter = null);
    }
}