using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.DataAccess.Abstract
{
    public interface IEntityRepository<T>
    {
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        IEnumerable<T> GetListEnumerable(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        int Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}