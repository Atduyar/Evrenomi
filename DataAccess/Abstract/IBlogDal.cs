using System.Collections.Generic;
using Core.DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace DataAccess.Abstract
{
    public interface IBlogDal:IEntityRepository<Blog>,IEntityPagedList<Blog>
    {
        List<Blog> GetSearchList(List<string> filters = null);
        List<Tag> GetTag(int blogId);
    }
}